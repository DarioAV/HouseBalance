using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;

namespace HouseBalance
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            InitApp();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                BalanceList.ItemsSource = App.SQLiteDB.GetDbBalances();
            }
            catch (Exception ex)
            {

            }
            
        }

        List<string> TipoDeGasto;

        private void InitApp()
        {
            TipoDeGasto = new List<string>();
            TipoDeGasto.Add("Gas");
            TipoDeGasto.Add("Luz");
            foreach (var item in TipoDeGasto)
            {
                Selector.Items.Add(item);
            }
        }

        public class PickerType
        {
            public string TipoDeGasto { get; set; }
        }

        private async void Button_Clicked(object sender, EventArgs e)  //Boton para insertar la cantidad y el tipo en el registro
        {
            if (DataValidation())
            {
                DbBalance balanceNumber = new DbBalance
                {
                    Amount = double.Parse(RemainingAmount.Text),
                    BalanceType = Selector.SelectedItem.ToString(),
                    CaptureDay = DateTime.UtcNow
                };
                App.SQLiteDB.SaveBalance(balanceNumber);
                RemainingAmount.Text = "";
                Selector.SelectedItem = "";
                await DisplayAlert("Registro", "Los datos se guardaron correctamente", "Listo");
                var balance = App.SQLiteDB.GetDbBalances();
                BalanceList.ItemsSource = balance;
                DateEnter.IsVisible = false;
            }
            else
            {
                await DisplayAlert("Cuidado", "Ingresa los datos antes de continuar", "Listo");
            }
        }

        private void Button_Add_Balance(object sender, EventArgs e)
        {
            DateEnter.IsVisible = true;
        }

        public bool DataValidation()  //Unicamente observa si se han rellenado los datos antes de registrarlos
        {
            bool respuesta = false;

            if (Selector.SelectedItem == null)
                return respuesta;

            if (string.IsNullOrWhiteSpace(Selector.SelectedItem.ToString()))
                return respuesta;

            if(string.IsNullOrWhiteSpace(RemainingAmount.Text))
                return respuesta;

            if(!double.TryParse(RemainingAmount.Text, out var number))
                return respuesta;

            return true;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            DateEnter.IsVisible = false;
        }
    }

    public class SQLiteHelper //Clase para la creacion de la base de datos
    {
        SQLiteConnection db;

        public SQLiteHelper(string dbPath)
        {
            try
            {
                db = new SQLiteConnection(dbPath);              
                db.CreateTable<DbBalance>();
            }
            catch (Exception ex)
            {

            }
        }

        public int SaveBalance(DbBalance balanceNumber)
        {
            if (balanceNumber.IdBalance == 0)
            {
                return db.Insert(balanceNumber);
            }
            return 0;
        }

        public List<DbBalance> GetDbBalances()
        {
            return db.Table<DbBalance>().ToList();
        }

        public DbBalance GetDbBalanceByIdAsync(int idBalance)
        {
            return db.Table<DbBalance>().FirstOrDefault(a => a.IdBalance == idBalance);
        }
    }

    public class DbBalance   //Clase creada para las propiedades del item de la base de datos
    {
        [PrimaryKey, AutoIncrement]
        public int IdBalance { get; set; }
        public double Amount { get; set; }
        public string BalanceType { get; set; }
        public DateTime CaptureDay { get; set; }
    }

    public class CustomEntry : Entry
    {
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create("CornerRadius", typeof(int), typeof(CustomEntry), 0);

        public int EntryCornerRadius
        {
            get { return (int)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty BorderColorproperty =
            BindableProperty.Create("BorderThickness", typeof(Color), typeof(CustomEntry), Color.White);

        public Color EntryBorderColor
        {
            get { return (Color)GetValue(BorderColorproperty); }
            set {  SetValue(BorderColorproperty, value); }
        }
    }
}
