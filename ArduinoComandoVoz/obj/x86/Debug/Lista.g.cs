﻿#pragma checksum "D:\IFSP\Modulo3\Programacao de Dispositivos Windows Phone - Fernando Salina\TrabalhoFinal\ArduinoComandoVoz\ArduinoComandoVoz\Lista.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4A4FD08CCAE272F4630303A6BEDBC7B8"
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
    partial class Lista : 
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
                    this.button_Copy = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 30 "..\..\..\Lista.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.button_Copy).Click += this.button_Click;
                    #line default
                }
                break;
            case 2:
                {
                    this.lstUsuario = (global::Windows.UI.Xaml.Controls.ListBox)(target);
                    #line 19 "..\..\..\Lista.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListBox)this.lstUsuario).SelectionChanged += this.lstUsuario_SelectionChanged;
                    #line default
                }
                break;
            case 3:
                {
                    this.txtBlTitulo = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 4:
                {
                    this.txtBlMensagem = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
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

