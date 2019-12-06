using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

// 有关程序集的一般信息由以下
// 控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("Plugin.Core")]
[assembly: AssemblyDescription("万物之源..其中包含GMTools的UI样式")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("舰队的偶像-岛风酱")]
[assembly: AssemblyProduct("Plugin.Core")]
[assembly: AssemblyCopyright("Copyright ©  2019 舰队的偶像-岛风酱, All Rights Reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
// 自定义XAML命名空间
[assembly: XmlnsDefinition("url://GM.Tools/ctrls", "Plugin.Controls")]
//[assembly: XmlnsDefinition("https://tools.GameModdle.org/ctrls", "Plugin.Controls")] // 不会启用的一个
// 将 ComVisible 设置为 false 会使此程序集中的类型
//对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型
//请将此类型的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

//若要开始生成可本地化的应用程序，请设置
//.csproj 文件中的 <UICulture>CultureYouAreCodingWith</UICulture>
//例如，如果您在源文件中使用的是美国英语，
//使用的是美国英语，请将 <UICulture> 设置为 en-US。  然后取消
//对以下 NeutralResourceLanguage 特性的注释。  更新
//以下行中的“en-US”以匹配项目文件中的 UICulture 设置。

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]


[assembly:ThemeInfo(
    ResourceDictionaryLocation.None, //主题特定资源词典所处位置
                             //(未在页面中找到资源时使用，
                             //或应用程序资源字典中找到时使用)
    ResourceDictionaryLocation.SourceAssembly //常规资源词典所处位置
                                      //(未在页面中找到资源时使用，
                                      //、应用程序或任何主题专用资源字典中找到时使用)
)]


// 程序集的版本信息由下列四个值组成: 
//
//      主版本
//      次版本
//      生成号
//      修订号
//
//可以指定所有这些值，也可以使用“生成号”和“修订号”的默认值
//通过使用 "*"，如下所示:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
