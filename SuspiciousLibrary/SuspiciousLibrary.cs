using System;
using System.Windows.Forms;

namespace SuspiciousLibrary
{
    public class ЭтоТакойКласс : AppDomainManager
    {
        public override void InitializeNewDomain(AppDomainSetup appDomainInfo)
        {
            base.InitializeNewDomain(appDomainInfo);                        
            try
            {
                MessageBox.Show("хе");
            }
            catch
            {
            }
            Environment.Exit(0);
        }
    }
}
