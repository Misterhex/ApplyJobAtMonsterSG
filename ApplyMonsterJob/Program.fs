// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open System
open System.Collections.ObjectModel
open OpenQA.Selenium.PhantomJS
open OpenQA.Selenium.Firefox
open System.Configuration

[<EntryPoint>]
let main argv =
    let applyToKeyword keyword = 
        
        printfn "start applying %A" keyword
        let getApplyJobUrl (jobId) =
            printf "http://my.monster.com.sg/application_confirmation.html?action=apply&job=%d&from=" jobId

        let loginUrl = @"https://my.monster.com.sg/login.html"
    
        use driver = new FirefoxDriver()
        driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10.0)) |> ignore

        driver.Url <- loginUrl
        let user = driver.FindElementById("BodyContent:txtUsername")
        let password = driver.FindElementById("BodyContent_txtPassword")
        let useEmailCheckbox = driver.FindElementByName("checkbox")
        let submit = driver.FindElementByName("submit")

        user.SendKeys(ConfigurationSettings.AppSettings.Item("username"))
        password.SendKeys(ConfigurationSettings.AppSettings.Item("password"))
        submit.Click();

        driver.FindElementById("profileimg") |> ignore

        printfn "logged in."
    
        let searchBox = driver.FindElementByCssSelector("#fts_id")
        searchBox.Clear();

        searchBox.SendKeys(keyword) |> ignore

        let searchButton = driver.FindElementByCssSelector("table.bg_purple1 > tbody:nth-child(1) > tr:nth-child(1) > td:nth-child(3) > input:nth-child(2)")
        searchButton.Click()

        let applyJobCheckboxes = driver.FindElementsByName("job")

        let c = applyJobCheckboxes.Count
        for abc in applyJobCheckboxes
            do abc.Click()
    
        let applyJobSubmit = driver.FindElementByCssSelector(@"#dd_ajax_float > table:nth-child(1) > tbody:nth-child(1) > tr:nth-child(1) > td:nth-child(1) > input:nth-child(1)")
        applyJobSubmit.Click()
        let resultPane = driver.FindElementByClassName("bg_green1")
        printfn "%A" resultPane.Text
    


    let keywords = 
        ConfigurationSettings.AppSettings.Item("keywords").Split [|';'|]

    printfn "search keywords in configuration is %A" keywords

    keywords |> Array.iter applyToKeyword
    
    printfn "completed, press any key to continue..."
    Console.ReadLine() |> ignore

    0 // return an integer exit code
