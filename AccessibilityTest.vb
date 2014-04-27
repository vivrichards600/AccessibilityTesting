Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Firefox

<TestClass()> _
Public Class AccessibilityTests

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Private _testContextInstance As TestContext
    Public Property TestContext() As TestContext
        Get
            Return _testContextInstance
        End Get
        Set(ByVal value As TestContext)
            _testContextInstance = value
        End Set
    End Property

    <TestMethod()> _
    Public Sub CheckPageAccessibilityTest()

        Dim fireFoxProfileManager = New FirefoxProfileManager
        Dim fireFoxBrowser = New FirefoxDriver(fireFoxProfileManager.GetProfile("SELENIUM"))

        fireFoxBrowser.Navigate.GoToUrl("https://twitter.com/11vlr")

        Dim browser = fireFoxBrowser.FindElement(By.TagName("body"))
        browser.SendKeys(Keys.Alt + "T")
        browser.SendKeys(Keys.ArrowDown)
        browser.SendKeys(Keys.ArrowDown)
        browser.SendKeys(Keys.ArrowDown)
        browser.SendKeys(Keys.ArrowRight)
        browser.SendKeys(Keys.Enter)

        Dim waveTips = fireFoxBrowser.FindElements(By.ClassName("wave4tip"))
        Assert.IsFalse(waveTips.Count = 0, "Could not locate any WAVE validations - please ensure that WAVE is installed correctly")

        Dim pageHasErrors As Boolean = False
        For Each waveTip In waveTips
            If waveTip.GetAttribute("alt").StartsWith("ERROR:") Then
                _testContextInstance.WriteLine(String.Format("Accessibility Error:{0}{1}{2}Description:{0}{3}", vbTab, waveTip.GetAttribute("alt").Replace("ERROR:", ""), vbCrLf, waveTip.GetAttribute("description")))
                pageHasErrors = True
            End If
        Next

        Assert.IsFalse(pageHasErrors = True, "WAVE errors were found on the page. Please check test output for details.")
        fireFoxBrowser.Close()
    End Sub
End Class
