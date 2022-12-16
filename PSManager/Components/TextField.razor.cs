using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;

namespace PSManager.Components
{
    public partial class TextField : InputBase<string>
    {
        [Inject]
        private IStringLocalizer<App> Loc { get; set; }

        [Parameter]
        public string? Id { get; set; }

        [Parameter]
        public string? LabelKey { get; set; }

        [Parameter]
        public string? DescriptionKey { get; set; }

        protected override bool TryParseValueFromString(string? value, out string result, out string validationErrorMessage)
        {
            result = value;
            validationErrorMessage = null;
            return true;
        }
    }
}