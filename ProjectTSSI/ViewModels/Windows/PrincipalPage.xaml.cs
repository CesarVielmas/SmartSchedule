using System.Reflection;
using Microsoft.Maui.Controls;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Microsoft.Maui.Layouts;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Net.Http;  // Para usar HttpClient
using System.Threading.Tasks;  // Para usar Task y Task<T>
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Globalization;
using System.Linq;

using Windows.UI.Input.Inking;

namespace ProjectTSSI
{
    public partial class PrincipalPage : ContentPage,INotifyPropertyChanged
    {
        private int _apartPage = 0;
        private StackLayout _currentPageContent;
        private readonly HttpClient _httpClient;
        private TaskProject taskEdit = new TaskProject();
        private bool isEditing = false;
        private List<TaskProject> listTasks = new List<TaskProject>();
        private string newTaskName = "";
        private string newTaskType = "";
        private string newTaskClassification = "";
        public string NewTaskName
        {
            get => newTaskName;
            set
            {
                if (newTaskName != value)
                {
                    newTaskName = value;
                    OnPropertyChanged();
                }
            }
        }
        public PrincipalPage()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://127.0.0.1:8000/");
            InitializeComponent();
            BindingContext = this;
            this.Loaded += OnPageLoaded;
            this.SizeChanged += OnWindowSizeChanged;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await FirstContentApering();
            var response = await _httpClient.GetStringAsync("tasks");  
            listTasks = JsonConvert.DeserializeObject<List<TaskProject>>(response);  
            listTasks.ForEach((t)=>{
                CreateTaskDinamic(t);
            });
            AddEventsInit();
            SelectMostLearning();
        }
        private void AddEventsInit(){
            var optionTypeTaskLayout1 = _currentPageContent.FindByName<StackLayout>("OptionTypeTaskLayout1");
            var optionTypeTaskLayout2 = _currentPageContent.FindByName<StackLayout>("OptionTypeTaskLayout2");
            var optionTypeTaskLayout3 = _currentPageContent.FindByName<StackLayout>("OptionTypeTaskLayout3");
            var optionTypeTaskLayout4 = _currentPageContent.FindByName<StackLayout>("OptionTypeTaskLayout4");
            var optionTypeTaskLayout5 = _currentPageContent.FindByName<StackLayout>("OptionTypeTaskLayout5");
            var optionTypeTaskLayout6 = _currentPageContent.FindByName<StackLayout>("OptionTypeTaskLayout6");

            var optionClassificationTaskLayout1 = _currentPageContent.FindByName<StackLayout>("OptionClassificationTaskLayout1");
            var optionClassificationTaskLayout2 = _currentPageContent.FindByName<StackLayout>("OptionClassificationTaskLayout2");
            var optionClassificationTaskLayout3 = _currentPageContent.FindByName<StackLayout>("OptionClassificationTaskLayout3");

            var buttonCreateTask = _currentPageContent.FindByName<Button>("ButtonCreateTask");
            
            optionTypeTaskLayout1.GestureRecognizers.Add(new TapGestureRecognizer{
                Command = new Command((sender) => OnClickedType(sender,EventArgs.Empty,"OptionTypeTaskLayout1"))  
            });
            optionTypeTaskLayout2.GestureRecognizers.Add(new TapGestureRecognizer{
                Command = new Command((sender) => OnClickedType(sender,EventArgs.Empty,"OptionTypeTaskLayout2"))  
            });
            optionTypeTaskLayout3.GestureRecognizers.Add(new TapGestureRecognizer{
                Command = new Command((sender) => OnClickedType(sender,EventArgs.Empty,"OptionTypeTaskLayout3"))  
            });
            optionTypeTaskLayout4.GestureRecognizers.Add(new TapGestureRecognizer{
                Command = new Command((sender) => OnClickedType(sender,EventArgs.Empty,"OptionTypeTaskLayout4"))  
            });
            optionTypeTaskLayout5.GestureRecognizers.Add(new TapGestureRecognizer{
                Command = new Command((sender) => OnClickedType(sender,EventArgs.Empty,"OptionTypeTaskLayout5"))  
            });
            optionTypeTaskLayout6.GestureRecognizers.Add(new TapGestureRecognizer{
                Command = new Command((sender) => OnClickedType(sender,EventArgs.Empty,"OptionTypeTaskLayout6"))  
            });
            optionClassificationTaskLayout1.GestureRecognizers.Add(new TapGestureRecognizer{
                Command = new Command((sender) => OnClickedClassification(sender,EventArgs.Empty,"OptionClassificationTaskLayout1"))  
            });
            optionClassificationTaskLayout2.GestureRecognizers.Add(new TapGestureRecognizer{
                Command = new Command((sender) => OnClickedClassification(sender,EventArgs.Empty,"OptionClassificationTaskLayout2"))  
            });
            optionClassificationTaskLayout3.GestureRecognizers.Add(new TapGestureRecognizer{
                Command = new Command((sender) => OnClickedClassification(sender,EventArgs.Empty,"OptionClassificationTaskLayout3"))  
            });
            buttonCreateTask.GestureRecognizers.Add(new TapGestureRecognizer{
                Command = new Command((sender) => OnClickedNewTask())  
            });
        }
        private void OnPageLoaded(object sender, EventArgs e)
        {
            SetWindowRestrictions();
            
                
        }
        private async Task FirstContentApering(){
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("ProjectTSSI.Views.Windows.PrincipalPageTask.xaml")){
                using (var reader = new StreamReader(stream)){
                    // Leer el contenido XAML
                    var xamlContent = reader.ReadToEnd();
                    var stackLayout = new StackLayout();
                    stackLayout.LoadFromXaml(xamlContent);

                    // Asegurarse de que el contenido tiene el tamaño correcto
                    stackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
                    stackLayout.VerticalOptions = LayoutOptions.FillAndExpand;

                    // Agregar el XAML al layout
                    _currentPageContent = stackLayout;
                    PrincipalContent.Children.Add(_currentPageContent);
                }
            }
            await Task.Yield();

            WindowsChanged();
        }
        private async Task SecondContent(){
             var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("ProjectTSSI.Views.Windows.PrincipalPageInProgress.xaml")){
                using (var reader = new StreamReader(stream)){
                    // Leer el contenido XAML
                    var xamlContent = reader.ReadToEnd();
                    var stackLayout = new StackLayout();
                    stackLayout.LoadFromXaml(xamlContent);

                    // Asegurarse de que el contenido tiene el tamaño correcto
                    stackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
                    stackLayout.VerticalOptions = LayoutOptions.FillAndExpand;

                    // Agregar el XAML al layout
                    _currentPageContent = stackLayout;
                    PrincipalContent.Children.Add(_currentPageContent);
                }
            }
            await Task.Yield();

        }
        private async void OnClickedIcons(object sender, EventArgs e){
            if (sender is StackLayout stackLayout){
                string elementName = stackLayout.AutomationId; // Usamos StyleId para identificarlo
                switch (elementName)
                {
                    case "ContainerProfileImage":
                        ImageProfileSelect.Source="icon_user_select.png";
                        ContainerTaskImage.BackgroundColor = Colors.Transparent;
                        ImageTaskSelect.Source="icon_navebar_task_unselect.png";
                        BorderTaskSelect.IsVisible = false;
                        ContainerStadisticsImage.BackgroundColor = Colors.Transparent;
                        ImageStadisticsSelect.Source="icon_static_unselect.png";
                        BorderStadisticsSelect.IsVisible = false;
                        ContainerComentsImage.BackgroundColor = Colors.Transparent;
                        ImageComentsSelect.Source="icon_comment_unselect.png";
                        BorderCommentsSelect.IsVisible = false;
                        ContainerAlarmsImage.BackgroundColor = Colors.Transparent;
                        ImageAlarmSelect.Source="icon_alarm_unselect.png";
                        BorderAlarmSelect.IsVisible = false;
                        ContainerConfigurationsImage.BackgroundColor = Colors.Transparent;
                        ImageConfigSelect.Source="icon_config_unselect.png";
                        BorderConfigSelect.IsVisible = false;
                        PrincipalContent.Children.Clear();
                        _apartPage = 1;
                        await SecondContent();
                        break;
                    case "ContainerTaskImage":
                        ContainerTaskImage.BackgroundColor = Color.FromArgb("#efeff4");
                        ImageTaskSelect.Source="icon_navebar_task_select.png";
                        BorderTaskSelect.IsVisible = true;
                        ImageProfileSelect.Source="icon_user_unselect.png";
                        ContainerStadisticsImage.BackgroundColor = Colors.Transparent;
                        ImageStadisticsSelect.Source="icon_static_unselect.png";
                        BorderStadisticsSelect.IsVisible = false;
                        ContainerComentsImage.BackgroundColor = Colors.Transparent;
                        ImageComentsSelect.Source="icon_comment_unselect.png";
                        BorderCommentsSelect.IsVisible = false;
                        ContainerAlarmsImage.BackgroundColor = Colors.Transparent;
                        ImageAlarmSelect.Source="icon_alarm_unselect.png";
                        BorderAlarmSelect.IsVisible = false;
                        ContainerConfigurationsImage.BackgroundColor = Colors.Transparent;
                        ImageConfigSelect.Source="icon_config_unselect.png";
                        BorderConfigSelect.IsVisible = false;
                        PrincipalContent.Children.Clear();
                        _apartPage = 0;
                        await FirstContentApering();
                        listTasks.ForEach((e)=>{
                            CreateTaskDinamic(e);
                        });
                        AddEventsInit();
                        SelectMostLearning();
                        break;
                    case "ContainerStadisticsImage":
                        ContainerStadisticsImage.BackgroundColor = Color.FromArgb("#efeff4");
                        ImageStadisticsSelect.Source="icon_static_select.png";
                        BorderStadisticsSelect.IsVisible = true;
                        ImageProfileSelect.Source="icon_user_unselect.png";
                        ContainerTaskImage.BackgroundColor = Colors.Transparent;
                        ImageTaskSelect.Source="icon_navebar_task_unselect.png";
                        BorderTaskSelect.IsVisible = false;
                        ContainerComentsImage.BackgroundColor = Colors.Transparent;
                        ImageComentsSelect.Source="icon_comment_unselect.png";
                        BorderCommentsSelect.IsVisible = false;
                        ContainerAlarmsImage.BackgroundColor = Colors.Transparent;
                        ImageAlarmSelect.Source="icon_alarm_unselect.png";
                        BorderAlarmSelect.IsVisible = false;
                        ContainerConfigurationsImage.BackgroundColor = Colors.Transparent;
                        ImageConfigSelect.Source="icon_config_unselect.png";
                        BorderConfigSelect.IsVisible = false;
                        PrincipalContent.Children.Clear();
                        _apartPage = 1;
                        await SecondContent();
                        break;
                    case "ContainerComentsImage":
                        ContainerComentsImage.BackgroundColor = Color.FromArgb("#efeff4");
                        ImageComentsSelect.Source="icon_comment_select.png";
                        BorderCommentsSelect.IsVisible = true;
                        ImageProfileSelect.Source="icon_user_unselect.png";
                        ContainerTaskImage.BackgroundColor = Colors.Transparent;
                        ImageTaskSelect.Source="icon_navebar_task_unselect.png";
                        BorderTaskSelect.IsVisible = false;
                        ContainerStadisticsImage.BackgroundColor = Colors.Transparent;
                        ImageStadisticsSelect.Source="icon_static_unselect.png";
                        BorderStadisticsSelect.IsVisible = false;
                        ContainerAlarmsImage.BackgroundColor = Colors.Transparent;
                        ImageAlarmSelect.Source="icon_alarm_unselect.png";
                        BorderAlarmSelect.IsVisible = false;
                        ContainerConfigurationsImage.BackgroundColor = Colors.Transparent;
                        ImageConfigSelect.Source="icon_config_unselect.png";
                        BorderConfigSelect.IsVisible = false;
                        PrincipalContent.Children.Clear();
                        _apartPage = 1;
                        await SecondContent();
                        break;
                    case "ContainerAlarmsImage":
                        ContainerAlarmsImage.BackgroundColor = Color.FromArgb("#efeff4");
                        ImageAlarmSelect.Source="icon_alarm_select.png";
                        BorderAlarmSelect.IsVisible = true;
                        ImageProfileSelect.Source="icon_user_unselect.png";
                        ContainerTaskImage.BackgroundColor = Colors.Transparent;
                        ImageTaskSelect.Source="icon_navebar_task_unselect.png";
                        BorderTaskSelect.IsVisible = false;
                        ContainerStadisticsImage.BackgroundColor = Colors.Transparent;
                        ImageStadisticsSelect.Source="icon_static_unselect.png";
                        BorderStadisticsSelect.IsVisible = false;
                        ContainerComentsImage.BackgroundColor = Colors.Transparent;
                        ImageComentsSelect.Source="icon_comment_unselect.png";
                        BorderCommentsSelect.IsVisible = false;
                        ContainerConfigurationsImage.BackgroundColor = Colors.Transparent;
                        ImageConfigSelect.Source="icon_config_unselect.png";
                        BorderConfigSelect.IsVisible = false;
                        PrincipalContent.Children.Clear();
                        _apartPage = 1;
                        await SecondContent();
                        break;
                    case "ContainerConfigurationsImage":
                        ContainerConfigurationsImage.BackgroundColor = Color.FromArgb("#efeff4");
                        ImageConfigSelect.Source="icon_config_select.png";
                        BorderConfigSelect.IsVisible = true;
                        ImageProfileSelect.Source="icon_user_unselect.png";
                        ContainerTaskImage.BackgroundColor = Colors.Transparent;
                        ImageTaskSelect.Source="icon_navebar_task_unselect.png";
                        BorderTaskSelect.IsVisible = false;
                        ContainerStadisticsImage.BackgroundColor = Colors.Transparent;
                        ImageStadisticsSelect.Source="icon_static_unselect.png";
                        BorderStadisticsSelect.IsVisible = false;
                        ContainerComentsImage.BackgroundColor = Colors.Transparent;
                        ImageComentsSelect.Source="icon_comment_unselect.png";
                        BorderCommentsSelect.IsVisible = false;
                        ContainerAlarmsImage.BackgroundColor = Colors.Transparent;
                        ImageAlarmSelect.Source="icon_alarm_unselect.png";
                        BorderAlarmSelect.IsVisible = false;
                        PrincipalContent.Children.Clear();
                        _apartPage = 1;
                        await SecondContent();
                        break;
                }
            }
            
        }
        private async void OnClickedType(object sender,EventArgs e,string elementName){
                var optionTypeTask1 = _currentPageContent.FindByName<Label>("OptionTypeTask1");
                var optionTypeTask2 = _currentPageContent.FindByName<Label>("OptionTypeTask2");
                var optionTypeTask3 = _currentPageContent.FindByName<Label>("OptionTypeTask3");
                var optionTypeTask4 = _currentPageContent.FindByName<Label>("OptionTypeTask4");
                var optionTypeTask5 = _currentPageContent.FindByName<Label>("OptionTypeTask5");
                var optionTypeTask6 = _currentPageContent.FindByName<Label>("OptionTypeTask6");

                var optionTypeTaskLayout1 = _currentPageContent.FindByName<StackLayout>("OptionTypeTaskLayout1");
                var optionTypeTaskLayout2 = _currentPageContent.FindByName<StackLayout>("OptionTypeTaskLayout2");
                var optionTypeTaskLayout3 = _currentPageContent.FindByName<StackLayout>("OptionTypeTaskLayout3");
                var optionTypeTaskLayout4 = _currentPageContent.FindByName<StackLayout>("OptionTypeTaskLayout4");
                var optionTypeTaskLayout5 = _currentPageContent.FindByName<StackLayout>("OptionTypeTaskLayout5");
                var optionTypeTaskLayout6 = _currentPageContent.FindByName<StackLayout>("OptionTypeTaskLayout6");
                switch (elementName)
                {
                    case "OptionTypeTaskLayout1":
                        optionTypeTaskLayout1.BackgroundColor = Color.FromArgb("#9cafec");
                        optionTypeTaskLayout2.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout3.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout4.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout5.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout6.BackgroundColor = Color.FromArgb("#e3e3e5");

                        optionTypeTask1.TextColor = Colors.White;
                        optionTypeTask2.TextColor = Colors.Black;
                        optionTypeTask3.TextColor = Colors.Black;
                        optionTypeTask4.TextColor = Colors.Black;
                        optionTypeTask5.TextColor = Colors.Black;
                        optionTypeTask6.TextColor = Colors.Black;
                        newTaskType = "Escuela";
                        break;
                    case "OptionTypeTaskLayout2":
                        optionTypeTaskLayout2.BackgroundColor = Color.FromArgb("#9cafec");
                        optionTypeTaskLayout1.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout3.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout4.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout5.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout6.BackgroundColor = Color.FromArgb("#e3e3e5");

                        optionTypeTask2.TextColor = Colors.White;
                        optionTypeTask1.TextColor = Colors.Black;
                        optionTypeTask3.TextColor = Colors.Black;
                        optionTypeTask4.TextColor = Colors.Black;
                        optionTypeTask5.TextColor = Colors.Black;
                        optionTypeTask6.TextColor = Colors.Black;
                        newTaskType = "Trabajo";
                        break;
                    case "OptionTypeTaskLayout3":
                        optionTypeTaskLayout3.BackgroundColor = Color.FromArgb("#9cafec");
                        optionTypeTaskLayout2.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout1.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout4.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout5.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout6.BackgroundColor = Color.FromArgb("#e3e3e5");

                        optionTypeTask3.TextColor = Colors.White;
                        optionTypeTask2.TextColor = Colors.Black;
                        optionTypeTask1.TextColor = Colors.Black;
                        optionTypeTask4.TextColor = Colors.Black;
                        optionTypeTask5.TextColor = Colors.Black;
                        optionTypeTask6.TextColor = Colors.Black;
                        newTaskType = "Deporte";
                        break;
                    case "OptionTypeTaskLayout4":
                        optionTypeTaskLayout4.BackgroundColor = Color.FromArgb("#9cafec");
                        optionTypeTaskLayout2.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout3.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout1.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout5.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout6.BackgroundColor = Color.FromArgb("#e3e3e5");

                        optionTypeTask4.TextColor = Colors.White;
                        optionTypeTask2.TextColor = Colors.Black;
                        optionTypeTask3.TextColor = Colors.Black;
                        optionTypeTask1.TextColor = Colors.Black;
                        optionTypeTask5.TextColor = Colors.Black;
                        optionTypeTask6.TextColor = Colors.Black;
                        newTaskType = "Hogar";
                        break;
                    case "OptionTypeTaskLayout5":
                        optionTypeTaskLayout5.BackgroundColor = Color.FromArgb("#9cafec");
                        optionTypeTaskLayout2.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout3.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout4.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout1.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout6.BackgroundColor = Color.FromArgb("#e3e3e5");

                        optionTypeTask5.TextColor = Colors.White;
                        optionTypeTask2.TextColor = Colors.Black;
                        optionTypeTask3.TextColor = Colors.Black;
                        optionTypeTask4.TextColor = Colors.Black;
                        optionTypeTask1.TextColor = Colors.Black;
                        optionTypeTask6.TextColor = Colors.Black;
                        newTaskType = "Compras";
                        break;
                    case "OptionTypeTaskLayout6":
                        optionTypeTaskLayout6.BackgroundColor = Color.FromArgb("#9cafec");
                        optionTypeTaskLayout2.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout3.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout4.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout5.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout1.BackgroundColor = Color.FromArgb("#e3e3e5");

                        optionTypeTask6.TextColor = Colors.White;
                        optionTypeTask2.TextColor = Colors.Black;
                        optionTypeTask3.TextColor = Colors.Black;
                        optionTypeTask4.TextColor = Colors.Black;
                        optionTypeTask5.TextColor = Colors.Black;
                        optionTypeTask1.TextColor = Colors.Black;
                        newTaskType = "Salidas";
                        break;
                }
            
        }
        private async void OnClickedClassification(object sender,EventArgs e,string elementName){
            var optionClassificationTaskLayout1 = _currentPageContent.FindByName<StackLayout>("OptionClassificationTaskLayout1");
            var optionClassificationTaskLayout2 = _currentPageContent.FindByName<StackLayout>("OptionClassificationTaskLayout2");
            var optionClassificationTaskLayout3 = _currentPageContent.FindByName<StackLayout>("OptionClassificationTaskLayout3");

            var optionClassificationTask1 = _currentPageContent.FindByName<Label>("OptionClassificationTask1");
            var optionClassificationTask2 = _currentPageContent.FindByName<Label>("OptionClassificationTask2");
            var optionClassificationTask3 = _currentPageContent.FindByName<Label>("OptionClassificationTask3");

             switch (elementName)
                {
                    case "OptionClassificationTaskLayout1":
                        optionClassificationTaskLayout1.BackgroundColor = Color.FromArgb("#9cafec");
                        optionClassificationTaskLayout2.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionClassificationTaskLayout3.BackgroundColor = Color.FromArgb("#e3e3e5");

                        optionClassificationTask1.TextColor = Colors.White;
                        optionClassificationTask2.TextColor = Colors.Black;
                        optionClassificationTask3.TextColor = Colors.Black;

                        newTaskClassification = "3";
                        break;
                    case "OptionClassificationTaskLayout2":
                        optionClassificationTaskLayout2.BackgroundColor = Color.FromArgb("#9cafec");
                        optionClassificationTaskLayout1.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionClassificationTaskLayout3.BackgroundColor = Color.FromArgb("#e3e3e5");

                        optionClassificationTask2.TextColor = Colors.White;
                        optionClassificationTask1.TextColor = Colors.Black;
                        optionClassificationTask3.TextColor = Colors.Black;

                        newTaskClassification = "2";
                        break;
                    case "OptionClassificationTaskLayout3":
                        optionClassificationTaskLayout3.BackgroundColor = Color.FromArgb("#9cafec");
                        optionClassificationTaskLayout2.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionClassificationTaskLayout1.BackgroundColor = Color.FromArgb("#e3e3e5");

                        optionClassificationTask3.TextColor = Colors.White;
                        optionClassificationTask2.TextColor = Colors.Black;
                        optionClassificationTask1.TextColor = Colors.Black;

                        newTaskClassification = "1";
                        break;
                   
                }
        }
        private async void SelectMostLearning(){
            var topTasks = new List<TaskProject>();
            var priorities = listTasks.Select(task => task.Prioridad).Distinct();
            foreach (var priority in priorities){
                var topTask = listTasks
                    .Where(task => task.Prioridad == priority)        
                    .OrderByDescending(task => task.QLearningPrioridad) 
                    .FirstOrDefault();                                

                if (topTask != null){
                    topTasks.Add(topTask);
                }
            }
            topTasks.ForEach((t)=>{
                if(t.Prioridad == 1){
                    var tittleMostImportantTask1 = _currentPageContent.FindByName<Label>("TittleMostImportantTask1");
                    var tittleMostImportantTask2 = _currentPageContent.FindByName<Label>("TittleMostImportantTask2");
                    var imageMostImportantTask1 = _currentPageContent.FindByName<Image>("ImageMostImportantTask1");
                    tittleMostImportantTask1.Text = t.Nombre;
                    tittleMostImportantTask2.Text = t.Fecha;
                    imageMostImportantTask1.Source = t.Tipo == "Escuela"?"icon_student.png":t.Tipo=="Trabajo"?"icon_job.png":t.Tipo=="Deporte"?"icon_deport.png":t.Tipo=="Hogar"?"icon_house.png":t.Tipo=="Compras"?"icon_shop.png":"icon_house_out.png";

                }
                else if(t.Prioridad == 2){
                    var tittleMostImportantTask3 = _currentPageContent.FindByName<Label>("TittleMostImportantTask3");
                    var tittleMostImportantTask4 = _currentPageContent.FindByName<Label>("TittleMostImportantTask4");
                    var imageMostImportantTask2 = _currentPageContent.FindByName<Image>("ImageMostImportantTask2");
                    tittleMostImportantTask3.Text = t.Nombre;
                    tittleMostImportantTask4.Text = t.Fecha;
                    imageMostImportantTask2.Source = t.Tipo == "Escuela"?"icon_student.png":t.Tipo=="Trabajo"?"icon_job.png":t.Tipo=="Deporte"?"icon_deport.png":t.Tipo=="Hogar"?"icon_house.png":t.Tipo=="Compras"?"icon_shop.png":"icon_house_out.png";
                }
                else if(t.Prioridad == 3){
                    var tittleMostImportantTask5 = _currentPageContent.FindByName<Label>("TittleMostImportantTask5");
                    var tittleMostImportantTask6 = _currentPageContent.FindByName<Label>("TittleMostImportantTask6");
                    var imageMostImportantTask3 = _currentPageContent.FindByName<Image>("ImageMostImportantTask1");
                    tittleMostImportantTask5.Text = t.Nombre;
                    tittleMostImportantTask6.Text = t.Fecha;
                    imageMostImportantTask3.Source = t.Tipo == "Escuela"?"icon_student.png":t.Tipo=="Trabajo"?"icon_job.png":t.Tipo=="Deporte"?"icon_deport.png":t.Tipo=="Hogar"?"icon_house.png":t.Tipo=="Compras"?"icon_shop.png":"icon_house_out.png";
                }
            });
        }
        private async void OnClickedNewTask(){
            if(!string.IsNullOrWhiteSpace(newTaskName) && !string.IsNullOrWhiteSpace(newTaskType) && !string.IsNullOrWhiteSpace(newTaskClassification)){
                var optionTypeTask1 = _currentPageContent.FindByName<Label>("OptionTypeTask1");
                var optionTypeTask2 = _currentPageContent.FindByName<Label>("OptionTypeTask2");
                var optionTypeTask3 = _currentPageContent.FindByName<Label>("OptionTypeTask3");
                var optionTypeTask4 = _currentPageContent.FindByName<Label>("OptionTypeTask4");
                var optionTypeTask5 = _currentPageContent.FindByName<Label>("OptionTypeTask5");
                var optionTypeTask6 = _currentPageContent.FindByName<Label>("OptionTypeTask6");

                var optionTypeTaskLayout1 = _currentPageContent.FindByName<StackLayout>("OptionTypeTaskLayout1");
                var optionTypeTaskLayout2 = _currentPageContent.FindByName<StackLayout>("OptionTypeTaskLayout2");
                var optionTypeTaskLayout3 = _currentPageContent.FindByName<StackLayout>("OptionTypeTaskLayout3");
                var optionTypeTaskLayout4 = _currentPageContent.FindByName<StackLayout>("OptionTypeTaskLayout4");
                var optionTypeTaskLayout5 = _currentPageContent.FindByName<StackLayout>("OptionTypeTaskLayout5");
                var optionTypeTaskLayout6 = _currentPageContent.FindByName<StackLayout>("OptionTypeTaskLayout6");

                var optionClassificationTaskLayout1 = _currentPageContent.FindByName<StackLayout>("OptionClassificationTaskLayout1");
                var optionClassificationTaskLayout2 = _currentPageContent.FindByName<StackLayout>("OptionClassificationTaskLayout2");
                var optionClassificationTaskLayout3 = _currentPageContent.FindByName<StackLayout>("OptionClassificationTaskLayout3");

                var optionClassificationTask1 = _currentPageContent.FindByName<Label>("OptionClassificationTask1");
                var optionClassificationTask2 = _currentPageContent.FindByName<Label>("OptionClassificationTask2");
                var optionClassificationTask3 = _currentPageContent.FindByName<Label>("OptionClassificationTask3");
                DateTime today = DateTime.Today;
                TaskProject taskPost = new TaskProject{
                    Nombre = newTaskName,
                    Prioridad = int.Parse(newTaskClassification),
                    Tipo = newTaskType,
                    QLearningPrioridad = 0,
                    Fecha = $"Creado El {today.Day} De {today.ToString("MMMM", new CultureInfo("es-ES"))} Del {today.Year}"
                };
                var jsonContent = JsonConvert.SerializeObject(taskPost);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                // Enviar el POST a la API
                var response = await _httpClient.PostAsync("tasks", content);  
                if (response.IsSuccessStatusCode){
                        DisplayAlert("TAREA","CREADA CON EXITO","OK");
                        newTaskName = "";
                        NewTaskName = "";
                        newTaskType = "";
                        newTaskClassification = "";
                        optionTypeTaskLayout1.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout2.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout3.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout4.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout5.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionTypeTaskLayout6.BackgroundColor = Color.FromArgb("#e3e3e5");

                        optionTypeTask1.TextColor = Colors.Black;
                        optionTypeTask2.TextColor = Colors.Black;
                        optionTypeTask3.TextColor = Colors.Black;
                        optionTypeTask4.TextColor = Colors.Black;
                        optionTypeTask5.TextColor = Colors.Black;
                        optionTypeTask6.TextColor = Colors.Black;

                        optionClassificationTaskLayout1.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionClassificationTaskLayout2.BackgroundColor = Color.FromArgb("#e3e3e5");
                        optionClassificationTaskLayout3.BackgroundColor = Color.FromArgb("#e3e3e5");

                        optionClassificationTask1.TextColor = Colors.Black;
                        optionClassificationTask2.TextColor = Colors.Black;
                        optionClassificationTask3.TextColor = Colors.Black;

                        listTasks.Add(taskPost);
                        _currentPageContent.FindByName<StackLayout>("ContentTaskDinamic").Children.Clear();
                        listTasks.ForEach((t)=>{
                            CreateTaskDinamic(t);
                        });
                }
                else{
                        DisplayAlert("ERROR","ERROR AL CREAR LA TAREA","OK");
                }
            }
        }
        private void SetWindowRestrictions(){
            // Obtén la ventana actual de la aplicación
            var mauiWindow = Application.Current.Windows[0].Handler.PlatformView as MauiWinUIWindow;
            var appWindow = mauiWindow?.AppWindow;
            if (appWindow != null){
                try{    
                    var presenter = appWindow.Presenter as OverlappedPresenter;
                    if (presenter != null){
                        presenter.IsResizable = true; // Bloquea el redimensionamiento de la ventana
                        presenter.IsMaximizable = false; //Bloquea la maximizacion
                        
                    }
                    
                    appWindow.Changed+= OnWindowChanged;
                    // Configura la ventana para que ocupe toda la pantalla
                    appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);

                    // Muestra la ventana
                    appWindow.Show();
                    
                }
                catch (Exception ex){
                    // Manejo de errores
                    System.Diagnostics.Debug.WriteLine($"Error al obtener la pantalla: {ex.Message}");
                }
            }
            
        }
        private void OnWindowChanged(AppWindow sender, AppWindowChangedEventArgs args){
           WindowsChanged();
        }
        private void OnWindowSizeChanged(object sender, EventArgs e){
           WindowsChanged();
        }
        private void WindowsChanged(){
             if(_apartPage == 0){
                var headerPart = _currentPageContent.FindByName<StackLayout>("HeaderPart");
                var headerTop = _currentPageContent.FindByName<StackLayout>("HeaderViewTop");
                var stackLayoutProgressBar = _currentPageContent.FindByName<StackLayout>("StackLayoutProgressBar");
                var progressBar = _currentPageContent.FindByName<ProgressBar>("ProgressBar");
                var progressBarText = _currentPageContent.FindByName<Label>("ProgressBarText");
                var mostImportantTextTittle = _currentPageContent.FindByName<Label>("MostImportantTextTittle");
                var mostImportantTextSubTittle = _currentPageContent.FindByName<Label>("MostImportantTextSubTittle");
                var textExpandTask = _currentPageContent.FindByName<Label>("TextExpandTask");
                var imageExpandTask = _currentPageContent.FindByName<Image>("ImageExpandTask");
                var imageNewTask = _currentPageContent.FindByName<Image>("ImageNewTask");
                var textNewTask = _currentPageContent.FindByName<Label>("TextNewTask");
                var imageTittleCompletedTask = _currentPageContent.FindByName<Image>("ImageTittleCompletedTask");
                var textTittleCompletedTask = _currentPageContent.FindByName<Label>("TextTittleCompletedTask");
                var imageTittleRecentTask = _currentPageContent.FindByName<Image>("ImageTittleRecentTask");
                var textTittleRecentTask = _currentPageContent.FindByName<Label>("TextTittleRecentTask");
                var textExpandTaskCompleted = _currentPageContent.FindByName<Label>("TextExpandTaskCompleted");
                var imageExpandTaskCompleted = _currentPageContent.FindByName<Image>("ImageExpandTaskCompleted");
                var textNameTask = _currentPageContent.FindByName<Label>("TextNameTask");
                var textTypeTask = _currentPageContent.FindByName<Label>("TextTypeTask");
                var frameNameTask = _currentPageContent.FindByName<Frame>("FrameNameTask");
                var entryNameTask = _currentPageContent.FindByName<Entry>("entryNameTask");
                var iconNameTask = _currentPageContent.FindByName<Image>("iconNameTask");
                var frameRecentTask = _currentPageContent.FindByName<Frame>("FrameRecentTask");
                var entryRecentTask = _currentPageContent.FindByName<Entry>("entryRecentTask");
                var iconRecentTask = _currentPageContent.FindByName<Image>("iconRecentTask");
                var textClassificationTask = _currentPageContent.FindByName<Label>("TextClassificationTask");
                var contentPart = _currentPageContent.FindByName<StackLayout>("ContentPart");
                var optionTypeTask1 = _currentPageContent.FindByName<Label>("OptionTypeTask1");
                var optionTypeTask2 = _currentPageContent.FindByName<Label>("OptionTypeTask2");
                var optionTypeTask3 = _currentPageContent.FindByName<Label>("OptionTypeTask3");
                var optionTypeTask4 = _currentPageContent.FindByName<Label>("OptionTypeTask4");
                var optionTypeTask5 = _currentPageContent.FindByName<Label>("OptionTypeTask5");
                var optionTypeTask6 = _currentPageContent.FindByName<Label>("OptionTypeTask6");
                var optionClassificationTask1 = _currentPageContent.FindByName<Label>("OptionClassificationTask1");
                var optionClassificationTask2 = _currentPageContent.FindByName<Label>("OptionClassificationTask2");
                var optionClassificationTask3 = _currentPageContent.FindByName<Label>("OptionClassificationTask3");
                var tittleMostImportantTask1 = _currentPageContent.FindByName<Label>("TittleMostImportantTask1");
                var tittleMostImportantTask2 = _currentPageContent.FindByName<Label>("TittleMostImportantTask2");
                var tittleMostImportantTask3 = _currentPageContent.FindByName<Label>("TittleMostImportantTask3");
                var tittleMostImportantTask4 = _currentPageContent.FindByName<Label>("TittleMostImportantTask4");
                var tittleMostImportantTask5 = _currentPageContent.FindByName<Label>("TittleMostImportantTask5");
                var tittleMostImportantTask6 = _currentPageContent.FindByName<Label>("TittleMostImportantTask6");
                var textTaskComplete1 = _currentPageContent.FindByName<Label>("TextTaskComplete1");
                var textTaskComplete2 = _currentPageContent.FindByName<Label>("TextTaskComplete2");
                var textTaskComplete3 = _currentPageContent.FindByName<Label>("TextTaskComplete3");
                var textTaskComplete4 = _currentPageContent.FindByName<Label>("TextTaskComplete4");
                var textTaskComplete5 = _currentPageContent.FindByName<Label>("TextTaskComplete5");
                var textTaskComplete6 = _currentPageContent.FindByName<Label>("TextTaskComplete6");
                var imageTaskComplete1 = _currentPageContent.FindByName<Image>("ImageTaskComplete1");
                var imageTaskComplete2 = _currentPageContent.FindByName<Image>("ImageTaskComplete2");
                var imageTaskComplete3 = _currentPageContent.FindByName<Image>("ImageTaskComplete3");
                var imageTaskComplete4 = _currentPageContent.FindByName<Image>("ImageTaskComplete4");
                var imageTaskComplete5 = _currentPageContent.FindByName<Image>("ImageTaskComplete5");
                var imageTaskComplete6 = _currentPageContent.FindByName<Image>("ImageTaskComplete6");
                var imageTaskComplete7 = _currentPageContent.FindByName<Image>("ImageTaskComplete7");
                var imageTaskComplete8 = _currentPageContent.FindByName<Image>("ImageTaskComplete8");
                var imageTaskComplete9 = _currentPageContent.FindByName<Image>("ImageTaskComplete9");
                var buttonCreateTask = _currentPageContent.FindByName<Button>("ButtonCreateTask");

                double parentWidth = PrincipalContent.Width;
                double parentHeight = PrincipalContent.Height;

                headerPart.WidthRequest = parentWidth - (headerPart.Margin.HorizontalThickness + PrincipalContent.Padding.HorizontalThickness) - 20;
                headerPart.HeightRequest = parentHeight * 0.3; 
                contentPart.WidthRequest = parentWidth - (contentPart.Margin.HorizontalThickness + PrincipalContent.Padding.HorizontalThickness) - 20;
                contentPart.HeightRequest = parentHeight * 0.6; 

                headerTop.HeightRequest = headerPart.Height * 0.26;
                progressBar.WidthRequest = stackLayoutProgressBar.Width * 0.9;
                progressBarText.FontSize = parentWidth * 0.0075;
                mostImportantTextSubTittle.FontSize = parentWidth * 0.0075;
                textExpandTask.FontSize = parentWidth * 0.0097;
                mostImportantTextTittle.FontSize = parentWidth * 0.0097;
                textTittleCompletedTask.FontSize = parentWidth * 0.01;
                textTittleRecentTask.FontSize = parentWidth * 0.01;
                textExpandTaskCompleted.FontSize = parentWidth * 0.0097;
                tittleMostImportantTask1.FontSize = parentWidth * 0.0097;
                tittleMostImportantTask2.FontSize = parentWidth * 0.0075;
                tittleMostImportantTask3.FontSize = parentWidth * 0.0097;
                tittleMostImportantTask4.FontSize = parentWidth * 0.0075;
                tittleMostImportantTask5.FontSize = parentWidth * 0.0097;
                tittleMostImportantTask6.FontSize = parentWidth * 0.0075;
                textTaskComplete1.FontSize = parentWidth * 0.008;
                textTaskComplete2.FontSize = parentWidth * 0.006;
                textTaskComplete3.FontSize = parentWidth * 0.008;
                textTaskComplete4.FontSize = parentWidth * 0.006;
                textTaskComplete5.FontSize = parentWidth * 0.008;
                textTaskComplete6.FontSize = parentWidth * 0.006;
                imageTaskComplete1.HeightRequest = parentHeight * 0.025;
                imageTaskComplete1.WidthRequest = parentWidth * 0.025;
                imageTaskComplete2.HeightRequest = parentHeight * 0.025;
                imageTaskComplete2.WidthRequest = parentWidth * 0.025;
                imageTaskComplete3.HeightRequest = parentHeight * 0.025;
                imageTaskComplete3.WidthRequest = parentWidth * 0.025;

                imageTaskComplete4.HeightRequest = parentHeight * 0.025;
                imageTaskComplete4.WidthRequest = parentWidth * 0.025;
                imageTaskComplete5.HeightRequest = parentHeight * 0.025;
                imageTaskComplete5.WidthRequest = parentWidth * 0.025;
                imageTaskComplete6.HeightRequest = parentHeight * 0.025;
                imageTaskComplete6.WidthRequest = parentWidth * 0.025;

                imageTaskComplete7.HeightRequest = parentHeight * 0.025;
                imageTaskComplete7.WidthRequest = parentWidth * 0.025;
                imageTaskComplete8.HeightRequest = parentHeight * 0.025;
                imageTaskComplete8.WidthRequest = parentWidth * 0.025;
                imageTaskComplete9.HeightRequest = parentHeight * 0.025;
                imageTaskComplete9.WidthRequest = parentWidth * 0.025;

                textNewTask.FontSize = parentWidth * 0.0097;
                imageNewTask.HeightRequest = parentHeight * 0.025;
                imageNewTask.WidthRequest = parentWidth * 0.025;
                
                imageExpandTaskCompleted.HeightRequest = parentHeight * 0.025;
                imageExpandTaskCompleted.WidthRequest = parentWidth * 0.025;
                imageExpandTask.HeightRequest = parentHeight * 0.025;
                imageExpandTask.WidthRequest = parentWidth * 0.025;
                imageTittleCompletedTask.HeightRequest = parentHeight * 0.03;
                imageTittleCompletedTask.WidthRequest = parentWidth * 0.03;
                imageTittleRecentTask.HeightRequest = parentHeight * 0.03;
                imageTittleRecentTask.WidthRequest = parentWidth * 0.03;
                frameNameTask.WidthRequest = contentPart.Width * 0.25;
                frameNameTask.HeightRequest = contentPart.HeightRequest * 0.11;
                frameRecentTask.WidthRequest = contentPart.Width * 0.25;
                frameRecentTask.HeightRequest = contentPart.HeightRequest * 0.11;
                entryNameTask.FontSize = contentPart.WidthRequest * 0.0085;
                entryRecentTask.FontSize = contentPart.WidthRequest * 0.0085;
                textNameTask.FontSize = parentWidth * 0.0075;
                textTypeTask.FontSize = parentWidth * 0.0075;
                textClassificationTask.FontSize = parentWidth * 0.0075;
                optionTypeTask1.FontSize = parentWidth * 0.009;
                optionTypeTask2.FontSize = parentWidth * 0.009;
                optionTypeTask3.FontSize = parentWidth * 0.009;
                optionTypeTask4.FontSize = parentWidth * 0.009;
                optionTypeTask5.FontSize = parentWidth * 0.009;
                optionTypeTask6.FontSize = parentWidth * 0.009;
                optionClassificationTask1.FontSize = parentWidth * 0.009;
                optionClassificationTask2.FontSize = parentWidth * 0.009;
                optionClassificationTask3.FontSize = parentWidth * 0.009;
                buttonCreateTask.HeightRequest = contentPart.Height * 0.14;
                buttonCreateTask.FontSize = parentWidth * 0.0097;
                AbsoluteLayout.SetLayoutBounds(iconNameTask, new Rect(contentPart.WidthRequest * 0.027, -(contentPart.HeightRequest * 0.072), 20, 20));
                AbsoluteLayout.SetLayoutBounds(iconRecentTask, new Rect(-(contentPart.WidthRequest * 0.07), -(contentPart.HeightRequest * 0.075), 15, 15));
                ContainerProfileImage.HeightRequest= PrincipalHeader.Height * 0.25;
                ContainerTaskImage.HeightRequest = PrincipalHeader.Height * 0.1;
                ContainerStadisticsImage.HeightRequest = PrincipalHeader.Height * 0.1;
                ContainerAlarmsImage.HeightRequest = PrincipalHeader.Height * 0.1;
                ContainerComentsImage.HeightRequest = PrincipalHeader.Height * 0.1;
                ContainerConfigurationsImage.HeightRequest = PrincipalHeader.Height * 0.1;

                ImageProfileSelect.HeightRequest = PrincipalHeader.Height * 0.05;
                ImageProfileSelect.HeightRequest = PrincipalHeader.Width * 0.4;

                ImageAlarmSelect.HeightRequest = PrincipalHeader.Height * 0.05;
                ImageAlarmSelect.HeightRequest = PrincipalHeader.Width * 0.4;

                ImageComentsSelect.HeightRequest = PrincipalHeader.Height * 0.05;
                ImageComentsSelect.HeightRequest = PrincipalHeader.Width * 0.4;

                ImageExpandSelect.HeightRequest = PrincipalHeader.Height * 0.05;
                ImageExpandSelect.HeightRequest = PrincipalHeader.Width * 0.4;

                ImageStadisticsSelect.HeightRequest = PrincipalHeader.Height * 0.05;
                ImageStadisticsSelect.HeightRequest = PrincipalHeader.Width * 0.4;

                ImageTaskSelect.HeightRequest = PrincipalHeader.Height * 0.05;
                ImageTaskSelect.HeightRequest = PrincipalHeader.Width * 0.4;

                ImageConfigSelect.HeightRequest = PrincipalHeader.Height * 0.05;
                ImageConfigSelect.HeightRequest = PrincipalHeader.Width * 0.4;

                
                
            }
            else if(_apartPage == 1){

            }
            else if(_apartPage == 2){

            }
            else if(_apartPage == 3){

            }
            else if(_apartPage == 4){

            }
            else if(_apartPage == 5){

            }
            else if(_apartPage == 6){

            }
        }
        
        private void CreateTaskDinamic(TaskProject task){
        var stackLayout = new StackLayout
        {
            HorizontalOptions = LayoutOptions.FillAndExpand,
            Margin = new Thickness(15, 0, 15, 0),
            HeightRequest = 100
        };

        var grid = new Grid
        {
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.FillAndExpand,
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(0.1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(0.9, GridUnitType.Star) }
            }
        };

        // Frame in the first column
        var frame = new Frame
        {
            VerticalOptions = LayoutOptions.FillAndExpand,
            Margin = new Thickness(5, 15, 5, 15),
            BorderColor = task.Prioridad == 1?Color.FromArgb("#7ac080"):task.Prioridad == 2?Color.FromArgb("#9cafec"):Color.FromArgb("#b06f6f"),
            CornerRadius = 10,
            BackgroundColor = task.Prioridad == 1?Color.FromArgb("#7ac080"):task.Prioridad == 2?Color.FromArgb("#9cafec"):Color.FromArgb("#b06f6f"),
            Content = new Label
            {
                Text = task.Prioridad == 1?"Opcional":task.Prioridad == 2?"Relevante":"Critica",
                FontAttributes = FontAttributes.Bold,
                FontFamily = "PoppinsBold",
                FontSize = 10,
                HorizontalTextAlignment = TextAlignment.Start,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Colors.White
            }
        };
        grid.Add(frame, 0, 0);

        // StackLayout in the second column
        var innerStackLayout = new StackLayout
        {
            VerticalOptions = LayoutOptions.FillAndExpand
        };

        // AbsoluteLayout for icons
        var absoluteLayout = new AbsoluteLayout
        {
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Start,
            WidthRequest = 120,
            HeightRequest = 15
        };

        var iconStudent = new Image
        {
            Source = task.Tipo == "Escuela"?"icon_student.png":task.Tipo=="Trabajo"?"icon_job.png":task.Tipo=="Deporte"?"icon_deport.png":task.Tipo=="Hogar"?"icon_house.png":task.Tipo=="Compras"?"icon_shop.png":"icon_house_out.png",
            Margin = new Thickness(7, 0, 0, 0)
        };
        AbsoluteLayout.SetLayoutBounds(iconStudent, new Rect(0, 0, 30, 30));
        AbsoluteLayout.SetLayoutFlags(iconStudent, AbsoluteLayoutFlags.PositionProportional);

        var iconEdit = new Image
        {
            Source = "icon_edit.png",
            Margin = new Thickness(7, 0, 0, 0)
        };
        iconEdit.GestureRecognizers.Add(new TapGestureRecognizer{
            Command = new Command(() => OnEditClicked(task))  // 'task.Id' es el ID de la tarea
        });
        AbsoluteLayout.SetLayoutBounds(iconEdit, new Rect(0.5, 0, 30, 30));
        AbsoluteLayout.SetLayoutFlags(iconEdit, AbsoluteLayoutFlags.PositionProportional);

        var iconDelete = new Image
        {
            Source = "icon_delete.png",
            Margin = new Thickness(7, 0, 0, 0)
        };
        iconDelete.GestureRecognizers.Add(new TapGestureRecognizer{
            Command = new Command(() => OnDeleteClicked(task.Id))  // 'task.Id' es el ID de la tarea
        });
        AbsoluteLayout.SetLayoutBounds(iconDelete, new Rect(1, 0, 30, 30));
        AbsoluteLayout.SetLayoutFlags(iconDelete, AbsoluteLayoutFlags.PositionProportional);

        absoluteLayout.Children.Add(iconStudent);
        absoluteLayout.Children.Add(iconEdit);
        absoluteLayout.Children.Add(iconDelete);
        innerStackLayout.Children.Add(absoluteLayout);

        // Labels
        innerStackLayout.Children.Add(new Label
        {
            Text = task.Nombre,
            Style = (Style)Application.Current.Resources["SubttitleLogin"],
            FontSize = 13,
            HorizontalTextAlignment = TextAlignment.Start,
            Margin = new Thickness(10, 0, 40, 10)
        });

        innerStackLayout.Children.Add(new Label
        {
            Text = task.Fecha,
            Style = (Style)Application.Current.Resources["SubttitleLogin"],
            TextColor = Color.FromArgb("#c0c0c0"),
            HorizontalTextAlignment = TextAlignment.Start,
            FontSize = 11,
            Margin = new Thickness(10, 13, 0, 10)
        });

        // Add StackLayout to the Grid
        grid.Add(innerStackLayout, 1, 0);

        // Add Grid to the main StackLayout
        stackLayout.Children.Add(grid);

        // Add the main StackLayout to your ContentPage
        _currentPageContent.FindByName<StackLayout>("ContentTaskDinamic").Children.Add(stackLayout);

        }
    
        private async void OnEditClicked(TaskProject task){
        taskEdit = task;

        // Rellenar los campos con los datos de la tarea seleccionada
        EntryNombre.Text = task.Nombre;
        EntryTipo.Text = task.Tipo;
        EntryPrioridad.Text = task.Prioridad.ToString();
        EntryFecha.Text = task.Fecha;
        EntryQLearningPrioridad.Text = task.QLearningPrioridad.ToString();
        
        // Hacer visible el formulario
        EditTaskPage.IsVisible = true;
        }
        private async void OnSaveClicked(object sender, EventArgs e){
            if (taskEdit != null){
                taskEdit.Nombre = EntryNombre.Text;
                taskEdit.Tipo = EntryTipo.Text;
                taskEdit.Prioridad = int.Parse(EntryPrioridad.Text);
                taskEdit.Fecha = EntryFecha.Text;

                var response = await _httpClient.PutAsJsonAsync($"tasks/{taskEdit.Id}", taskEdit);
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Tarea guardada exitosamente", "OK");
                    _currentPageContent.FindByName<StackLayout>("ContentTaskDinamic").Children.Clear();
                    listTasks.ForEach((t)=>{
                        CreateTaskDinamic(t);
                    });
                    taskEdit = null;
                    EditTaskPage.IsVisible = false; 
                }
                else
                {
                    await DisplayAlert("Error", "Hubo un problema al guardar la tarea", "OK");
                }
            }
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            EditTaskPage.IsVisible = false; // Ocultar el formulario
        }

        private async void OnDeleteClicked(int taskId){
            bool isConfirmed = await DisplayAlert(
            "Confirmación", 
            "¿Estás seguro de que deseas eliminar esta tarea?", 
            "Sí", 
            "No");
            if(isConfirmed){
                var response = await _httpClient.DeleteAsync($"tasks/{taskId}");
                listTasks.RemoveAll(task => task.Id == taskId);
                _currentPageContent.FindByName<StackLayout>("ContentTaskDinamic").Children.Clear();
                listTasks.ForEach((t)=>{
                CreateTaskDinamic(t);
                });
            }
            
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
