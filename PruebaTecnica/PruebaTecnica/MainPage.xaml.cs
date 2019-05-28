using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;

namespace PruebaTecnica
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

        }

        async void GetWeather(object sender, EventArgs args)
        {
           
            string uri = "http://api.openweathermap.org/data/2.5/weather?";
            string sCity = city.Text;
            string q = "q=" + sCity;
            string sCountry = country.Items[country.SelectedIndex];

            if (sCountry.Equals("España")) {
                q += ",es";
             } else if (sCountry.Equals("United Kingdom"))
            {
                q += ",uk";
            }

            string appid = "&APPID=4a1e12e2460756ffcd2063739625b8ba";
            string units = "&units=metric";
            string lang = "&lang=es";
            string finalUri = uri + q + appid + units + lang;

            //Console.WriteLine("Llamada al servicio: " + finalUri);

            try
            {
                HttpClient client = new HttpClient();
                //client.Timeout = TimeSpan.FromMilliseconds(System.Threading.Timeout.Infinite);
                var response = await client.GetAsync(finalUri);
                //Console.WriteLine("## 1 ##");
                /*  "weather": [{"description": "cielo claro"
                    "main": {"temp": 24.59,
                */
                if (response.IsSuccessStatusCode == true)
                {
                    //Console.WriteLine("## 2 ##");
                    var content = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine("## 3 ##");
                    //Console.WriteLine("## " + content.ToString());

                    var jsonData = JsonConvert.DeserializeObject<OpenWeahterApi>(content.ToString());

                    //Console.WriteLine("## 4 ##");
                    List<Weather> w = jsonData.weather;
                    //Console.WriteLine("########## " + w.ToString());

                    Weather we = w[0];
                    Main m = jsonData.main;
                    string cityName = jsonData.name;

                    Console.WriteLine("#################### salida ####################");
                    Console.WriteLine("El tiempo en: " + cityName + " es " + we.description);
                    //Console.WriteLine("El tiempo en: " + cityName );
                    Console.WriteLine("Y la temperatura es de: " + m.temp + " grados.");
                    Console.WriteLine("#################### salida ####################");

                    lbWeather.Text = "El tiempo en " + cityName + " es " + we.description + " con una temperatura de " + m.temp + " grados.";

                } else if (response.IsSuccessStatusCode == false)
                {

                    var contentError = await response.Content.ReadAsStringAsync();
                    var jsonDataError = JsonConvert.DeserializeObject<OpenWeahterApi>(contentError.ToString());
                    string statusCode = jsonDataError.cod.ToString();

                    if (statusCode.Equals("404"))
                    {
                        Console.WriteLine("Error 404 ciudad no encontrada");

                        await DisplayAlert("Alerta", "NO SE HA ENCONTRADO LA CIUDAD ESPECIFICADA, " + sCity + " EN EL PAIS " + sCountry, "Cerrar");
                    }
                    else
                    {
                        await DisplayAlert("Alerta", "Revise la ciudad y el país para mostrar el tiempo.", "Cerrar");
                    }

                    Console.WriteLine("# ERROR STATUS CODE # " + statusCode + " -- ");
                    
                }

            }  
                catch (Exception exception)

            {
                Console.WriteLine(exception.Message + " ### " + exception.ToString() + " #### " + exception.Data.Values.ToString());
                throw new Exception(exception.Message, exception);
            }

        }

        }

 }
