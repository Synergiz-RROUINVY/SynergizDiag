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

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page https://go.microsoft.com/fwlink/?LinkId=234236

namespace SynergizDiag
{
    public sealed partial class TextBlockPair : UserControl
    {
        public TextBlockPair()
        {
            this.InitializeComponent();
        }

        public String Text1
        {
            get
            {
                return TxBk1.Text;
            }
            set
            {
                TxBk1.Text = value;
            }
        }

        public String Text2
        {
            get
            {
                return TxBk2.Text;
            }
            set
            {
                TxBk2.Text = value;
            }
        }

    }
}
