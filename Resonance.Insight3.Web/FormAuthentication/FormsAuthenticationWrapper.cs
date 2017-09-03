using System.Web;
using System.Web.Security;
using Resonance.Insight3.Web.CertonaService;

namespace Resonance.Insight3.Web.FormAuthentication
{
    public class FormsAuthenticationWrapper : IFormsAuthentication
    {
        /// <summary>
        ///     Returns current login user credentials
        /// </summary>
        public static UserDTO User
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session["user"] != null)
                {
                    return (UserDTO) HttpContext.Current.Session["user"];
                }
                return null;
            }
            set { HttpContext.Current.Session["user"] = value; }
        }

        /// <summary>
        ///     Stores any session related information
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="createPersistentCookie"></param>
        public void SetAuthCookie(UserDTO user, bool createPersistentCookie)
        {
            HttpContext.Current.Session["user"] = user;
            FormsAuthentication.SetAuthCookie(user.Name, createPersistentCookie);
        }

        /// <summary>
        ///     Invokes FormsAuthentication Signout and clears session related objects if any
        /// </summary>
        public void SignOut()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.RemoveAll();
        }
    }

    public interface IFormsAuthentication
    {
        void SetAuthCookie(UserDTO credentials, bool createPersistentCookie);

        void SignOut();
    }
}