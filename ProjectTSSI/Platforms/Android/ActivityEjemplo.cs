using Android.App;
using Android.OS;
using Android.Widget;

namespace ProjectTSSI
{
    [Activity(Label = "Actividad Ejemplo")]
    public class ActivityEjemplo : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layouts.activity_ejemplo);

            // Obtén los controles desde el layout
            var textView = FindViewById<TextView>(Resource.Id.textView1);
            var button = FindViewById<Button>(Resource.Id.button1);

            button.Click += (sender, e) =>
            {
                textView.Text = "Botón presionado!";
            };
        }
    }
}
