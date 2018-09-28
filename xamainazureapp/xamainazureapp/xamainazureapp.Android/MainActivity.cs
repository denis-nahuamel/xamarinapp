using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Media;
using Android.OS;
using Android.Support.V4.App;
using Firebase.Iid;
using Firebase.Messaging;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace xamainazureapp.Droid
{
    [Activity(Label = "Omega Doctor", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            string IdPatient = Intent.GetStringExtra("idPatient");//captura del id del paciente
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            if (IdPatient == null )
            {
                LoadApplication(new App(false));
            }
            else
            {
                App.IdNotificationPatient = IdPatient;

                LoadApplication(new App(true));
            }
            //LoadApplication(new App());
            //FirebasePushNotificationManager.ProcessIntent(this, Intent);
            // In a "real" app you will have to deal with it if services are unavailable!
            FirebaseMessaging.Instance.SubscribeToTopic("hello");
            IsPlayServicesAvailable();

            #if DEBUG
                        // Force refresh of the token. If we redeploy the app, no new token will be sent but the old one will
                        // be invalid.
                        Task.Run(() =>
                        {
                            // This may not be executed on the main thread.
                            FirebaseInstanceId.Instance.DeleteInstanceId();
                            Console.WriteLine("Forced token: " + FirebaseInstanceId.Instance.Token);
                        });
#endif
            //var x = typeof(Xamarin.Forms.Themes.LightThemeResources);
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    // In a real project you can give the user a chance to fix the issue.
                    Console.WriteLine($"Error: {GoogleApiAvailability.Instance.GetErrorString(resultCode)}");
                }
                else
                {
                    Console.WriteLine("Error: Play services not supported!");
                    Finish();
                }
                return false;
            }
            else
            {
                Console.WriteLine("Play Services available.");
                return true;
            }
        }
    
    [Service]
        [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
        public class MyFirebaseIIDService : FirebaseInstanceIdService
        {
            public override void OnTokenRefresh()
            {
                var refreshedToken = FirebaseInstanceId.Instance.Token;
                Console.WriteLine($"Token received: {refreshedToken}");
                SendRegistrationToServerAsync(refreshedToken);
            }

            async Task SendRegistrationToServerAsync(string token)
            {
                try
                {
                    // Formats: https://firebase.google.com/docs/cloud-messaging/concept-options
                    // The "notification" format will automatically displayed in the notification center if the 
                    // app is not in the foreground.
                     const string templateBodyFCM =
                         "{" +
                             "\"data\" : {" +//antes notification
                             "\"message\" : \"$(messageParam)\"," +//antes body
                               "\"idDoctor\" : \"$(idDoctor)\"," +
                                "\"idPatient\" : \"$(idPatient)\"," +
                                "\"idReport\" : \"$(idRepor)\"," +
                             "\"icon\" : \"myicon\" }" +
                         "}";
                    //const string templateBodyFCM = "{\"data\":{\"message\":\"$(messageParam)\"}}";
                var templates = new JObject();
                    templates["genericMessage"] = new JObject
                {
                    {"body", templateBodyFCM}
                };

                    var client = new MobileServiceClient(xamainazureapp.App.MobileServiceUrl);
                    var push = client.GetPush();

                    await push.RegisterAsync(token, templates);

                    // Push object contains installation ID afterwards.
                    Console.WriteLine(push.InstallationId.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                   // Debugger.Break();
                }
            }
        }

        // This service is used if app is in the foreground and a message is received.
        [Service]
        [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
        public class MyFirebaseMessagingService : FirebaseMessagingService
        {
            public override void OnMessageReceived(RemoteMessage message)
            {
                base.OnMessageReceived(message);

                Console.WriteLine("Received: " + message);

                // Android supports different message payloads. To use the code below it must be something like this (you can paste this into Azure test send window):
                // {
                //   "notification" : {
                //      "body" : "The body",
                //                 "title" : "The title",
                //                 "icon" : "myicon
                //   }
                // }
                try
                {
                    //var msg = message.GetNotification().Body;
                    var msg = message.Data["message"];
                    var name = message.Data["idReport"];
                    var idPatient= message.Data["idPatient"];
                    //MessagingCenter.Send<object, string>(this,"notificacion recibida", msg);
                    //var msgReceiver = (AndroidMessageReceiver)DependencyService.Get<IMessagingCenter>();
                    //msgReceiver.Handle(msg);
                    var intent = new Intent(this, typeof(MainActivity));
                    
                    intent.AddFlags(ActivityFlags.ClearTop);
                    intent.PutExtra("idPatient",idPatient);
                    var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

                    var notificationBuilder = new NotificationCompat.Builder(this)
                        .SetSmallIcon(Resource.Drawable.abc_ic_star_black_48dp)
                        .SetContentTitle("Nuevo Reporte")
                        .SetContentText(msg)
                        .SetContentIntent(pendingIntent)
                        .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                        //.set
                        .SetAutoCancel(true);
                    

                    var notificationManager = NotificationManager.FromContext(this);
                    notificationManager.Notify(0, notificationBuilder.Build());
                    
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error extracting message: " + ex);
                }
            }

        }
    }
}

