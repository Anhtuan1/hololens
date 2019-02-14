//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using PeerConnectionClient.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WindowsSocketWrapper;
using System.Diagnostics;
using Windows.UI.Input;
using Windows.Data.Json;
using Windows.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
#if UNITY
using UnityPlayer;
#endif

namespace PeerConnectionClient
{
    /// <summary>
    /// The application main page.
    /// </summary>

    public class ObjectModel
    {
        public float x;
        public float y;
    }
    public sealed partial class MainPage : Page
    {
        Dictionary<uint, Windows.UI.Xaml.Input.Pointer> pointers;
        private MainViewModel _mainViewModel;
        private SocketWrapper socketWrapper;
        private bool flag_set = false;
        
        private bool draw_status = false;
        private bool arraw_status = false;


        //send image 
        Byte[] picAttachment = new Byte[0];
        string name_image_midd = "";
        Point screenCoords = new Point();
        int rotate_x = 0;
        float resize_x = 1;


        public PointCollection points = new PointCollection();
#if UNITY
        private WinRTBridge.WinRTBridge _bridge;

        private SplashScreen splash;
        private Rect splashImageRect;
        private WindowSizeChangedEventHandler onResizeHandler;
        private bool isPhone = false;
#endif

        /// <summary>
        /// Constructor.
        /// </summary>
        private void OnReceivedMessage_handler(string msg)
        {
            try
            {
                Debug.WriteLine(msg);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        

            private void objectManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var stackDragged = e.OriginalSource as StackPanel;
            (stackDragged.RenderTransform as TranslateTransform).X += e.Delta.Translation.X;
            (stackDragged.RenderTransform as TranslateTransform).Y += e.Delta.Translation.Y;
        }
       


        private void OnDisconnection_handler(bool connect)
        {
            try
            {
                if (!connect)
                {
                    socketWrapper.Connect("ws://192.168.50.132:3001/socket.io/?EIO=4&transport=websocket");
                }
                else
                {
                    Debug.WriteLine("Successfully Disconnect");
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public MainPage()
        {
            this.InitializeComponent();
            pointers = new Dictionary<uint, Windows.UI.Xaml.Input.Pointer>();
            VideoBorder.PointerMoved += new PointerEventHandler(VideoBorder_PointerMoved);
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;


            try
            {
                // connect to socket.io via other server
                socketWrapper = new SocketWrapper();

                socketWrapper.OnReceiveMessenger += OnReceivedMessage_handler;
                socketWrapper.onDisconnect += OnDisconnection_handler;
                Debug.WriteLine("TEst Connect");
                socketWrapper.Connect("ws://192.168.50.132:3001/socket.io/?EIO=4&transport=websocket");
                Debug.WriteLine("after connect");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

#if UNITY
            AppCallbacks appCallbacks = AppCallbacks.Instance;
            // Setup scripting bridge
            _bridge = new WinRTBridge.WinRTBridge();
            appCallbacks.SetBridge(_bridge);

            bool isWindowsHolographic = false;

#if UNITY_HOLOGRAPHIC
            // If application was exported as Holographic check if the deviceFamily actually supports it,
            // otherwise we treat this as a normal XAML application
            string deviceFamily = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;
            isWindowsHolographic = String.Compare("Windows.Holographic", deviceFamily) == 0;
#endif

            if (isWindowsHolographic)
            {
                appCallbacks.InitializeViewManager(Window.Current.CoreWindow);
            }
            else
            {
                appCallbacks.RenderingStarted += () => { RemoveSplashScreen(); };

#if UNITY_UWP
                if (Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1))
                    isPhone = true;
#endif
                appCallbacks.SetSwapChainPanel(GetSwapChainPanel());
                appCallbacks.SetCoreWindowEvents(Window.Current.CoreWindow);
                appCallbacks.InitializeD3DXAML();

                //splash = ((App)App.Current).splashScreen;
                GetSplashBackgroundColor();
                OnResize();
                onResizeHandler = new WindowSizeChangedEventHandler((o, e) => OnResize());
                Window.Current.SizeChanged += onResizeHandler;
            }
            PeerVideo.Visibility = Visibility.Collapsed;
            SelfVideo.Visibility = Visibility.Collapsed;
#else
            DXSwapChainPanel.Visibility = Visibility.Collapsed;
#endif // UNITY
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _mainViewModel = (MainViewModel)e.Parameter;
            this.DataContext = _mainViewModel;
#if UNITY
            //splash = (SplashScreen)e.Parameter;
            OnResize();
#else
            _mainViewModel.PeerVideo = PeerVideo;
            _mainViewModel.SelfVideo = SelfVideo;
#endif

        }

#if UNITY
        private void OnResize()
        {
            if (splash != null)
            {
                splashImageRect = splash.ImageLocation;
                PositionImage();
            }
        }

        private void PositionImage()
        {
            var inverseScaleX = 1.0f;
            var inverseScaleY = 1.0f;
            if (isPhone)
            {
                inverseScaleX = inverseScaleX / DXSwapChainPanel.CompositionScaleX;
                inverseScaleY = inverseScaleY / DXSwapChainPanel.CompositionScaleY;
            }

            //ExtendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.X * inverseScaleX);
            //ExtendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Y * inverseScaleY);
            //ExtendedSplashImage.Height = splashImageRect.Height * inverseScaleY;
            //ExtendedSplashImage.Width = splashImageRect.Width * inverseScaleX;
        }

        private async void GetSplashBackgroundColor()
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///AppxManifest.xml"));
                string manifest = await FileIO.ReadTextAsync(file);
                int idx = manifest.IndexOf("SplashScreen");
                manifest = manifest.Substring(idx);
                idx = manifest.IndexOf("BackgroundColor");
                if (idx < 0)  // background is optional
                    return;
                manifest = manifest.Substring(idx);
                idx = manifest.IndexOf("\"");
                manifest = manifest.Substring(idx + 1);
                idx = manifest.IndexOf("\"");
                manifest = manifest.Substring(0, idx);
                int value = 0;
                bool transparent = false;
                if (manifest.Equals("transparent"))
                    transparent = true;
                else if (manifest[0] == '#') // color value starts with #
                    value = Convert.ToInt32(manifest.Substring(1), 16) & 0x00FFFFFF;
                else
                    return; // at this point the value is 'red', 'blue' or similar, Unity does not set such, so it's up to user to fix here as well
                byte r = (byte)(value >> 16);
                byte g = (byte)((value & 0x0000FF00) >> 8);
                byte b = (byte)(value & 0x000000FF);

                await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.High, delegate ()
                {
                    byte a = (byte)(transparent ? 0x00 : 0xFF);
                    //ExtendedSplashGrid.Background = new SolidColorBrush(Color.FromArgb(a, r, g, b));
                });
            }
            catch (Exception)
            { }
        }

        public SwapChainPanel GetSwapChainPanel()
        {
            return DXSwapChainPanel;
        }

        public void RemoveSplashScreen()
        {
            //DXSwapChainPanel.Children.Remove(ExtendedSplashGrid);
            //if (onResizeHandler != null)
            //{
            //    Window.Current.SizeChanged -= onResizeHandler;
            //    onResizeHandler = null;
            //}
        }
#endif

        /// <summary>
        /// Invoked when the Add button is clicked 
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the exception routed event.</param>
        private void ConfirmAddButton_Click(object sender, RoutedEventArgs e)
        {
            this.AddButton.Flyout.Hide();
        }

        /// <summary>
        /// Media Failed event handler for remote/peer video.
        /// Invoked when an error occurs in peer media source.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the exception routed event.</param>
        private void PeerVideo_MediaFailed(object sender, Windows.UI.Xaml.ExceptionRoutedEventArgs e)
        {
            if (_mainViewModel != null)
            {
                _mainViewModel.PeerVideo_MediaFailed(sender, e);
            }
        }

        /// <summary>
        /// Media Failed event handler for self video.
        /// Invoked when an error occurs in self media source.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the exception routed event.</param>
        private void SelfVideo_MediaFailed(object sender, Windows.UI.Xaml.ExceptionRoutedEventArgs e)
        {
            if (_mainViewModel != null)
            {
                _mainViewModel.SelfVideo_MediaFailed(sender, e);
            }
        }

        private void Draw_star(object sender, RoutedEventArgs e)
        {
            draw_status = true;
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Person, 0);
            Action_event.Visibility = Visibility.Collapsed;
            Action_event1_2.Visibility = Visibility.Visible;
           
        }
        private void Draw_star2(object sender, RoutedEventArgs e)
        {
            draw_status = false;
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
            Action_event.Visibility = Visibility.Visible;
            Action_event1_2.Visibility = Visibility.Collapsed;
        }

        private void Arraw_star(object sender, RoutedEventArgs e)
        {
            arraw_status = true;
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
            Action_event_21.Visibility = Visibility.Visible;
            Action_event_2.Visibility = Visibility.Collapsed;
        }

        private void Arraw_star2(object sender, RoutedEventArgs e)
        {
            arraw_status = false;
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
            Action_event_2.Visibility = Visibility.Visible;
            Action_event_21.Visibility = Visibility.Collapsed;
        }



        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
        }

        private async void Grid_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Any())
                {
                    var storageFile = items[0] as StorageFile;
                    var bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(await storageFile.OpenAsync(FileAccessMode.Read));
                    //DroppedImage.Source = bitmapImage;
                    //drag_image.Width = bitmapImage.PixelWidth;
                    //drag_image.Height = bitmapImage.PixelHeight;
                }
            }
        }
        private void Button_send_image(object sender, TappedRoutedEventArgs e)
        {
            
            // Convert the byte array to Base 64 string
            string base64String = Convert.ToBase64String(picAttachment);
            socketWrapper.SendMessage("mess", "base64position" + screenCoords.ToString() + ","  + ImagePreview.ActualWidth + "," + ImagePreview.ActualHeight+","+ rotate_x+","+resize_x);
            Debug.WriteLine("base64position"+screenCoords.ToString());
            Debug.WriteLine(ImagePreview.ActualHeight);
            Debug.WriteLine(ImagePreview.ActualWidth);
            socketWrapper.SendMessage("mess", "base64string" + base64String);    
            
            ImagePreview.Children.Clear();
            Action_event_9.Visibility = Visibility.Collapsed;
            Action_event_5.Visibility = Visibility.Collapsed;
            Action_event_6.Visibility = Visibility.Collapsed;
            Action_event_7.Visibility = Visibility.Collapsed;
            Action_event_8.Visibility = Visibility.Collapsed;
            CompositeTransform ct = ImagePreview.RenderTransform as CompositeTransform;
            if (ct == null) ImagePreview.RenderTransform = ct = new CompositeTransform();
            ct.Rotation = 0;
            ct.ScaleX = 1;
            ct.ScaleY = 1;
            rotate_x = 0;
            resize_x = 1;
        }

        private void Plus_image(object sender, RoutedEventArgs e)
        {
            if (name_image_midd != "") {
                
                CompositeTransform ct = ImagePreview.RenderTransform as CompositeTransform;
                if (ct == null) ImagePreview.RenderTransform = ct = new CompositeTransform();
                ct.ScaleX += 0.12;
                ct.ScaleY += 0.12;
                resize_x = resize_x + 0.12f;
            }
        }

        private void Minus_image(object sender, RoutedEventArgs e)
        {
            if (name_image_midd != "")
            {
                CompositeTransform ct = ImagePreview.RenderTransform as CompositeTransform;
                if (ct == null) ImagePreview.RenderTransform = ct = new CompositeTransform();
                ct.ScaleX -= 0.12;
                ct.ScaleY -= 0.12;
                resize_x = resize_x - 0.12f;
            }
        }

        private void rotate_image_left(object sender, RoutedEventArgs e)
        {
            if (name_image_midd != "")
            {
                CompositeTransform ct = ImagePreview.RenderTransform as CompositeTransform;
                if (ct == null) ImagePreview.RenderTransform = ct = new CompositeTransform();
                ct.Rotation -= 15;
                rotate_x = rotate_x - 15;
            }
        }

        private void rotate_image_right(object sender, RoutedEventArgs e)
        {
            if (name_image_midd != "")
            {
                
                CompositeTransform ct = ImagePreview.RenderTransform as CompositeTransform;
                if (ct == null) ImagePreview.RenderTransform = ct = new CompositeTransform();
                ct.Rotation += 15;
                rotate_x = rotate_x + 15;
            }
        }


        private async void SelectImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ImagePreview.Children.Clear();
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");

            StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {

                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                var image = new BitmapImage();                
                image.SetSource(stream);
                Image myImage1 = new Image();
                
                myImage1.Source = image;
                //Debug.WriteLine("px2 " + (((float) image.PixelHeight / (float)image.PixelWidth) ).ToString());
                myImage1.Width = 400;
                myImage1.Height = (int) (((float)image.PixelHeight /((float)image.PixelWidth))*400);
                screenCoords = new Point(400, (int) ((float)image.PixelHeight / ((float)image.PixelWidth)) * 400+792);
                Debug.WriteLine("screenCoords"+ screenCoords.ToString());
                myImage1.Stretch = Stretch.UniformToFill;
                myImage1.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;                
                myImage1.ManipulationCompleted += Img_ManipulationCompleted;
                myImage1.ManipulationStarted += Img_ManipulationStarted;
                myImage1.Name = "image_add_" + stream.ToString();
                name_image_midd = "image_add_" + stream.ToString();
                myImage1.ManipulationDelta += image_ManipulationDelta;
                                
                ImagePreview.Children.Add(myImage1);
                //var ttv = ImagePreview.TransformToVisual(Window.Current.Content);
                
                
                
                
                Action_event_9.Visibility = Visibility;
                Action_event_5.Visibility = Visibility;
                Action_event_6.Visibility = Visibility;
                Action_event_7.Visibility = Visibility;
                Action_event_8.Visibility = Visibility;

                // Create a byte array for the image
                picAttachment = new Byte[0];
                var reader = new DataReader(stream.GetInputStreamAt(0));
                picAttachment = new Byte[stream.Size];
                await reader.LoadAsync((uint)stream.Size);
                reader.ReadBytes(picAttachment);

            }
        }



        void Img_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {

            Image image = sender as Image;
            image.Opacity = 0.5;
        }

        void image_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Image image = sender as Image;
            CompositeTransform ct = ImagePreview.RenderTransform as CompositeTransform;
            if (ct == null) ImagePreview.RenderTransform = ct = new CompositeTransform();
            ct.TranslateX += e.Delta.Translation.X;
            screenCoords.X = (int) ct.TranslateX;
            ct.TranslateY += e.Delta.Translation.Y;
            screenCoords.Y = (int) ct.TranslateY;
           // Debug.WriteLine("screenCoords2" + screenCoords.ToString());
        }

        void Img_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e, CompositeTransform mytransform)
        {
            
            mytransform.TranslateX += e.Delta.Translation.X;
            mytransform.TranslateY += e.Delta.Translation.Y;
        }

        void Img_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            Image image = sender as Image;
            image.Opacity = 1;
        }


        private void star_send(object sender, PointerRoutedEventArgs e)
        {
            points = new PointCollection();
            if (draw_status == true)
            {
                flag_set = true;
                socketWrapper.SendMessage("mess", "Start_move");
               
            }
            if (arraw_status == true)
            {
                
                try
                {
                    PointerPoint ptrPt = e.GetCurrentPoint(VideoBorder);
                    ObjectModel obj = new ObjectModel();

                    int coorX = (int)ptrPt.Position.X;
                    int coorY = (int)ptrPt.Position.Y;
                    if (coorY < 0) { coorY = 0; }
                    if (coorY > 792) { coorY = 792; }
                    string mesg = "Start_arrow {" + string.Format("x:{0},y:{1}", coorX, coorY) + "}";
                    //LoadImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/1234321.png"));
                    //var ct = (CompositeTransform)LoadImage.RenderTransform;
                    //ct.TranslateX = coorX - 25;
                    //ct.TranslateY = coorY - 50;
                    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Pin, 0);
                    socketWrapper.SendMessage("mess", mesg);
                }
                catch (Exception ex)
                {
                    //Debug.WriteLine(ex.Message);
                }
            }
        }

        private void end_send(object sender, PointerRoutedEventArgs e)
        {
            line_demo.Points = new PointCollection();
            flag_set = false;
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
            socketWrapper.SendMessage("mess", "End_move");
        }

        private void VideoBorder_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                PointerPoint ptrPt = e.GetCurrentPoint(VideoBorder);
                ObjectModel obj = new ObjectModel();

                int coorX = (int)ptrPt.Position.X;
                int coorY = (int)ptrPt.Position.Y;
                if (coorY < 0) { coorY = 0; }
                if (coorY > 746) { coorY = 746; }
                string mesg = "{" + string.Format("x:{0},y:{1}", coorX, coorY) + "}";
                if (flag_set == true)
                {
                    
                    points.Add(new Windows.Foundation.Point(coorX, (int)ptrPt.Position.Y));
                    line_demo.Points = points;
                    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Pin, 0);
                    socketWrapper.SendMessage("mess", mesg);
                }
                // socketWrapper.SendMessage("mess", mesg);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
    }
}
