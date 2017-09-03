using System.ComponentModel.DataAnnotations;

namespace Resonance.Insight3.Web.ViewModels
{
    /// <summary>
    ///     Login View Model
    /// </summary>
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            SuccessMessage = string.Empty;
        }
        /// <summary>
        ///     User name
        /// </summary>
        [Required(ErrorMessageResourceType = typeof (Resource), ErrorMessageResourceName = "UserIDRequiredText")]
        public string UserName { get; set; }

        /// <summary>
        ///     Password
        /// </summary>
        [Required(ErrorMessageResourceType = typeof (Resource), ErrorMessageResourceName =  "PasswordRequiredText")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string SuccessMessage { get; set; }
    }
}