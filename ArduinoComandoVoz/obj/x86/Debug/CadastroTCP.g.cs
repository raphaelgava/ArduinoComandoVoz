﻿#pragma checksum "D:\IFSP\Modulo3\Programacao de Dispositivos Windows Phone - Fernando Salina\TrabalhoFinal\ArduinoComandoVoz\ArduinoComandoVoz\CadastroTCP.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A41387F69AA819B469A4C78986944729"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ArduinoComandoVoz
{
    partial class CadastroTCP : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.btnOk = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 20 "..\..\..\CadastroTCP.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnOk).Click += this.btnOk_Click;
                    #line default
                }
                break;
            case 2:
                {
                    this.btnVoltar = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 21 "..\..\..\CadastroTCP.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnVoltar).Click += this.button_Click;
                    #line default
                }
                break;
            case 3:
                {
                    this.imgLeds = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 22 "..\..\..\CadastroTCP.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.imgLeds).PointerReleased += this.imgLeds_PointerReleased;
                    #line default
                }
                break;
            case 4:
                {
                    this.textBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5:
                {
                    this.txtBHost = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 6:
                {
                    this.txtHost = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 7:
                {
                    this.txtBPort = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 8:
                {
                    this.txtPort = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

