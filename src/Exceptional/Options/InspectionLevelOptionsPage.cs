﻿namespace ReSharper.Exceptional.MF.Options
{
    using JetBrains.Application.Settings;
    using JetBrains.Application.UI.Options;
    using JetBrains.Application.UI.Options.OptionsDialog;
    using JetBrains.DataFlow;
    using JetBrains.IDE.UI.Options;
    using JetBrains.Lifetimes;

    using Settings;

    [OptionsPage(Pid, Name, typeof(UnnamedThemedIcons.ExceptionalSettings), ParentId = ExceptionalOptionsPage.Pid, Sequence = 1.0)]
    public class InspectionLevelOptionsPage : BeSimpleOptionsPage
    {
        #region constants

        public const string Name = "Inspection Level";
        public const string Pid = "Exceptional::InspectionLevel";

        #endregion

        #region constructors and destructors

        public InspectionLevelOptionsPage(
            Lifetime lifetime,
            OptionsPageContext optionsPageContext,
            OptionsSettingsSmartContext optionsSettingsSmartContext,
            bool wrapInScrollablePanel = false) : base(lifetime, optionsPageContext, optionsSettingsSmartContext, wrapInScrollablePanel)
        {
            CreateCheckboxInspectPublic(lifetime, optionsSettingsSmartContext.StoreOptionsTransactionContext);
            CreateCheckboxInspectInternal(lifetime, optionsSettingsSmartContext.StoreOptionsTransactionContext);
            CreateCheckboxInspectProtected(lifetime, optionsSettingsSmartContext.StoreOptionsTransactionContext);
            CreateCheckboxInspectPrivate(lifetime, optionsSettingsSmartContext.StoreOptionsTransactionContext);
            CreateCheckboxInspectConstructors(lifetime, optionsSettingsSmartContext.StoreOptionsTransactionContext);
        }

        #endregion

        #region methods

        private void CreateCheckboxInspectInternal(Lifetime lifetime, IContextBoundSettingsStoreLive storeOptionsTransactionContext)
        {
            IProperty<bool> property = new Property<bool>(lifetime, "Exceptional::InspectionLevel::InspectInternalMethodsAndProperties");
            property.SetValue(storeOptionsTransactionContext.GetValue((ExceptionalSettings key) => key.InspectInternalMethods));
            property.Change.Advise(
                lifetime,
                a =>
                {
                    if (!a.HasNew)
                    {
                        return;
                    }
                    storeOptionsTransactionContext.SetValue((ExceptionalSettings key) => key.InspectInternalMethods, a.New);
                });
            AddBoolOption((ExceptionalSettings key) => key.InspectInternalMethods, OptionsLabels.InspectionLevel.InspectInternalMethodsAndProperties);
        }

        private void CreateCheckboxInspectPrivate(Lifetime lifetime, IContextBoundSettingsStoreLive storeOptionsTransactionContext)
        {
            IProperty<bool> property = new Property<bool>(lifetime, "Exceptional::InspectionLevel::InspectPrivateMethodsAndProperties");
            property.SetValue(storeOptionsTransactionContext.GetValue((ExceptionalSettings key) => key.InspectPrivateMethods));
            property.Change.Advise(
                lifetime,
                a =>
                {
                    if (!a.HasNew)
                    {
                        return;
                    }
                    storeOptionsTransactionContext.SetValue((ExceptionalSettings key) => key.InspectPrivateMethods, a.New);
                });
            AddBoolOption((ExceptionalSettings key) => key.InspectPrivateMethods, OptionsLabels.InspectionLevel.InspectPrivateMethodsAndProperties);
        }

        private void CreateCheckboxInspectProtected(Lifetime lifetime, IContextBoundSettingsStoreLive storeOptionsTransactionContext)
        {
            IProperty<bool> property = new Property<bool>(lifetime, "Exceptional::InspectionLevel::InspectProtectedMethodsAndProperties");
            property.SetValue(storeOptionsTransactionContext.GetValue((ExceptionalSettings key) => key.InspectProtectedMethods));
            property.Change.Advise(
                lifetime,
                a =>
                {
                    if (!a.HasNew)
                    {
                        return;
                    }
                    storeOptionsTransactionContext.SetValue((ExceptionalSettings key) => key.InspectProtectedMethods, a.New);
                });
            AddBoolOption((ExceptionalSettings key) => key.InspectProtectedMethods, OptionsLabels.InspectionLevel.InspectProtectedMethodsAndProperties);
        }

        private void CreateCheckboxInspectPublic(Lifetime lifetime, IContextBoundSettingsStoreLive storeOptionsTransactionContext)
        {
            IProperty<bool> property = new Property<bool>(lifetime, "Exceptional::InspectionLevel::InspectPublicMethodsAndProperties");
            property.SetValue(storeOptionsTransactionContext.GetValue((ExceptionalSettings key) => key.InspectPublicMethods));
            property.Change.Advise(
                lifetime,
                a =>
                {
                    if (!a.HasNew)
                    {
                        return;
                    }
                    storeOptionsTransactionContext.SetValue((ExceptionalSettings key) => key.InspectPublicMethods, a.New);
                });
            AddBoolOption((ExceptionalSettings key) => key.InspectPublicMethods, OptionsLabels.InspectionLevel.InspectPublicMethodsAndProperties);
        }

        private void CreateCheckboxInspectConstructors(Lifetime lifetime, IContextBoundSettingsStoreLive storeOptionsTransactionContext)
        {
            IProperty<bool> property = new Property<bool>(lifetime, "Exceptional::InspectionLevel::InspectConstructorsAndProperties");
            property.SetValue(storeOptionsTransactionContext.GetValue((ExceptionalSettings key) => key.InspectConstructors));
            property.Change.Advise(
                lifetime,
                a =>
                {
                    if (!a.HasNew)
                    {
                        return;
                    }
                    storeOptionsTransactionContext.SetValue((ExceptionalSettings key) => key.InspectConstructors, a.New);
                });
            AddBoolOption((ExceptionalSettings key) => key.InspectConstructors, OptionsLabels.InspectionLevel.InspectConstructorsProperties);
        }

        #endregion
    }
}