Description::
    /// <summary>
    /// <para>GetValueByKey takes a key and adds .txt to it in order to create</para>
    /// <para>a file name. It gets the app data path and adds \IdealAutomate\yourscriptname</para>
    /// <para>to it. By combining that path to the file name created from the key,</para>
    /// <para>it can retrieve a value from the key that is unique to your script application.</para>
    ///  <para>The AppDirectory allows you to store personal settings and</para>
    /// <para>information that you want to keep private (like passwords) in a location</para>
    /// <para>outside of your script on in the application directory</para>
    /// </summary>
    /// <param name="strKey">Unique key within the script application</param>
    /// <returns>string that was in application directory for that key</returns>

Variables::
Input:
[[Key]]={{myKey}}
Output:
[[Value]]={{myValue}}


Syntax::
[[Value]] = myActions.GetValueByKey([[Key]]);

Examples::
 myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorTitlex");
  myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue");