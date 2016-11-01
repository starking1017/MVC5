using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC5.Models
{
  public class ExternalLoginConfirmationViewModel
  {
    [Required]
    [Display(Name = "Email")]
    public string Email { get; set; }
  }

  public class ExternalLoginListViewModel
  {
    public string ReturnUrl { get; set; }
  }

  public class SendCodeViewModel
  {
    public string SelectedProvider { get; set; }
    public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    public string ReturnUrl { get; set; }
    public bool RememberMe { get; set; }
  }

  public class VerifyCodeViewModel
  {
    [Required]
    public string Provider { get; set; }

    [Required]
    [Display(Name = "Code")]
    public string Code { get; set; }
    public string ReturnUrl { get; set; }

    [Display(Name = "记住浏览器?")]
    public bool RememberBrowser { get; set; }

    public bool RememberMe { get; set; }
  }

  public class ForgotViewModel
  {
    [Required]
    [Display(Name = "Email")]
    public string Email { get; set; }
  }

  public class LoginViewModel
  {
    [Required]
    [Display(Name = "账户名称")]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "密码")]
    public string Password { get; set; }

    [Display(Name = "记住?")]
    public bool RememberMe { get; set; }
  }

  public class AdminViewModel
  {
    [Required]
    [Display(Name = "账号")]
    public string Account { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "密码")]
    public string Password { get; set; }

  }

  public class RegisterViewModel
  {
    [Required]
    [Display(Name = "账户名称")]
    public string UserName { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "{0} 必须至少有 {2} 码.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "密码")]
    public string Password { get; set; }

    [Required]
    [Display(Name = "Email")]
    [EmailAddress]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "确认密码")]
    [Compare("Password", ErrorMessage = "密码和确认密码不相符.")]
    public string ConfirmPassword { get; set; }
  }

  public class ResetPasswordViewModel
  {
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "{0} 必须至少有 {2} 码.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "密码")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "确认密码")]
    [Compare("Password", ErrorMessage = "密码和确认密码不相符.")]
    public string ConfirmPassword { get; set; }

    public string Code { get; set; }
  }

  public class ForgotPasswordViewModel
  {
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }
  }
}

