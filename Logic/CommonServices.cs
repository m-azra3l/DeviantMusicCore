using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviantMusicCore.Logic;
using DeviantMusicCore.Models;

namespace DeviantMusicCore.Logic
{
    public class CommonServices
    {
        public static string ShowAlert(Alerts obj,string message)
        {
            string alertDiv = null;

            switch(obj)
            {
                case Alerts.Success:
                    alertDiv = "<div class='alert alert-success alert-dismissible' role='alert' id='alert'><button type='button' class='close' data-dismiss='alert'>&times;</button> Success! " + message+"</div><br/>";
                    break;
                case Alerts.Danger:
                    alertDiv = "<div class='alert alert-danger alert-dismissible' role='alert' id='alert'><button type='button' class='close' data-dismiss='alert'>&times;</button>Error! " + message + "</div><br/>";
                    break;
                case Alerts.Info:
                    alertDiv = "<div class='alert alert-info alert-dismissible' role='alert' id='alert'><button type='button' class='close' data-dismiss='alert'>&times;</button>Info! " + message + "</div><br/>";
                    break;
                case Alerts.Warning:
                    alertDiv = "<div class='alert alert-warning alert-dismissible' role='alert' id='alert'><button type='button' class='close' data-dismiss='alert'>&times;</button>Warning! " + message + "</div><br/>";
                    break;
            }
            
            return alertDiv;
        }       
    }
}
