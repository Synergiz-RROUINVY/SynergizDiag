using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace SynergizDiag
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class SearchDevices : Page
    {
        public SearchDevices()
        {
            this.InitializeComponent();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {

            for (int i = 1; i < 255; i++)
            {
                //-2147012867
                try
                {
                    //HoloLensAPIHelper.SetConnectivity(Enums.HoloLensConnectedBy.WiFi,)
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
