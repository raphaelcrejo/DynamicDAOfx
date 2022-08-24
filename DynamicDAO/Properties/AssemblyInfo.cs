using System.Reflection;
using System.Runtime.InteropServices;

// As informações gerais sobre um assembly são controladas por
// conjunto de atributos. Altere estes valores de atributo para modificar as informações
// associada a um assembly.
[assembly: AssemblyTitle("DynamicDAO")]
[assembly: AssemblyDescription("Provides an easy way to access databases and fill objects.")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCompany("Raphael da Cunha Crejo")]
[assembly: AssemblyProduct("DynamicDAO")]
[assembly: AssemblyCopyright("Copyright © 2017 Raphael da Cunha Crejo.")]
[assembly: AssemblyCulture("")]

// Definir ComVisible como false torna os tipos neste assembly invisíveis
// para componentes COM. Caso precise acessar um tipo neste assembly de
// COM, defina o atributo ComVisible como true nesse tipo.
[assembly: ComVisible(false)]

// O GUID a seguir será destinado à ID de typelib se este projeto for exposto para COM
[assembly: Guid("6078f77d-7b8c-44ab-b5ef-3febe6f516d0")]

// As informações da versão de um assembly consistem nos quatro valores a seguir:
//
//      Versão Principal
//      Versão Secundária 
//      Número da Versão
//      Revisão
//
// É possível especificar todos os valores ou usar como padrão os Números de Build e da Revisão
// usando o "*" como mostrado abaixo:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyInformationalVersion("1.0.0-rc1")]
