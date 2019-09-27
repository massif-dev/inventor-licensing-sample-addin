using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Inventor;
using Microsoft.Win32;
using Massif.Licensing;

namespace MassifExampleAddin
{
    /// <summary>
    /// This is the primary AddIn Server class that implements the ApplicationAddInServer interface
    /// that all Inventor AddIns are required to implement. The communication between Inventor and
    /// the AddIn is via the methods on this interface.
    /// </summary>
    [GuidAttribute("573e1c9f-6fa9-4799-9afc-c4f0ad2b9d44")]
    public class StandardAddInServer : Inventor.ApplicationAddInServer
    {

        // Inventor application object.
        private Inventor.Application m_inventorApplication;

        public StandardAddInServer()
        {
        }

        #region ApplicationAddInServer Members

        private ButtonDefinition runBtnDef = null;
        private UserInterfaceEvents uiEvents = null;

        public void Activate(Inventor.ApplicationAddInSite addInSiteObject, bool firstTime)
        {
            //MASSIF NOTES: Probably best not to put license checks here, otherwise you might slow down the Inventor startup.

            // This method is called by Inventor when it loads the addin.
            // The AddInSiteObject provides access to the Inventor Application object.
            // The FirstTime flag indicates if the addin is loaded for the first time.

            // Initialize AddIn members.
            m_inventorApplication = addInSiteObject.Application;

            //Create some UI
            Ribbon partRibbon = m_inventorApplication.UserInterfaceManager.Ribbons["ZeroDoc"];
            RibbonTab massifTab = partRibbon.RibbonTabs.Add("Massif SDK", "MASSIF_TAB_SAMPLE", Guid.NewGuid().ToString());
            RibbonPanel massifPanel = massifTab.RibbonPanels.Add("Massif Sample Tool", "PNL_MASSIF_SAMPLE", Guid.NewGuid().ToString());

            //Add a control definition for a button
            ControlDefinitions controlDefs = m_inventorApplication.CommandManager.ControlDefinitions;

            //Sort out the icon
            var btnImage = Properties.Resources.MassifIcon;
            stdole.IPictureDisp iPictDisp = PictureDispConverter.ToIPictureDisp(btnImage);

            //Create button definition
            runBtnDef = controlDefs.AddButtonDefinition("Run Test",
                "{6C426CFE-E9BC-44CB-8B46-F2D4BDE05AE6}",
                CommandTypesEnum.kQueryOnlyCmdType,
                "573e1c9f-6fa9-4799-9afc-c4f0ad2b9d44",
                "Test Massif licensing",
                "Click to check that you have a license to run this addin.",
                iPictDisp,
                iPictDisp,
                ButtonDisplayEnum.kDisplayTextInLearningMode
                );

            //Add button to panel
            CommandControl startControl = massifPanel.CommandControls.AddButton(runBtnDef, true, true);
            uiEvents = m_inventorApplication.UserInterfaceManager.UserInterfaceEvents;

            try
            {
                runBtnDef.OnExecute += new ButtonDefinitionSink_OnExecuteEventHandler(RunBtnDef_OnExecute);
            }

            catch
            {

            }

        }

        private void RunBtnDef_OnExecute(NameValueMap context)
        {
            //Do license check here

            //After installing the Massif.Licensing nuget package, add "using Massif.Licensing;" at the top of this file
            
            //Create a license connector
            Connector licenseConnector = new Connector();

            //Get the asset info from the Massif licensing engine
            Connector.IAssetInfo assetInfo = licenseConnector.GetAssetInfo("cps-jobber");

            //Check that license is valid, if not then return
            if (!assetInfo.LicenseActive)
            {
                MessageBox.Show("The addin could not load, as a valid license could not be found. Please ensure that the Massif Desktop App is running, and that you have a valid license, then restart the addin.");
                return;
            }

            MessageBox.Show("The addin runs, which means you have a valid license! Massif achievement, well done!");
        }


        public void Deactivate()
        {
            // This method is called by Inventor when the AddIn is unloaded.
            // The AddIn will be unloaded either manually by the user or
            // when the Inventor session is terminated

            // TODO: Add ApplicationAddInServer.Deactivate implementation

            // Release objects.
            m_inventorApplication = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void ExecuteCommand(int commandID)
        {
            // Note:this method is now obsolete, you should use the 
            // ControlDefinition functionality for implementing commands.
        }

        public object Automation
        {
            // This property is provided to allow the AddIn to expose an API 
            // of its own to other programs. Typically, this  would be done by
            // implementing the AddIn's API interface in a class and returning 
            // that class object through this property.

            get
            {
                // TODO: Add ApplicationAddInServer.Automation getter implementation
                return null;
            }
        }

        #endregion

    }
}
