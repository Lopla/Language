import * as vscode from 'vscode';
import { LoplaMethods } from "./intelisense";
import { SignatureHelp, SignatureInformation } from 'vscode';

export class FunctionCompletetionProvider implements vscode.SignatureHelpProvider{
    
    provideSignatureHelp(document: vscode.TextDocument, position: vscode.Position, 
      token: vscode.CancellationToken, context: vscode.SignatureHelpContext): 
      vscode.ProviderResult<vscode.SignatureHelp> 
    {
      var result:SignatureHelp = new SignatureHelp();
      
      let p = position.translate(0, -position.character);
      let line = document.getText(new vscode.Range(p, position));

      for (var key of Object.keys(LoplaMethods)) {
        let rs =  (key).replace(".", "[.]").toString();
        let r= new RegExp(rs);
        
        if( line && r.test(line)){

          var mdString = "";
          var methods = Object.values(LoplaMethods[key]);
          methods.forEach(element => {
            
            mdString = mdString + "* "+element.toString().trim()+"\n";
          });

          var signature = new SignatureInformation(key, new vscode.MarkdownString(mdString));

          result.signatures.push(signature);

          
          return result;
        }
      }
      
      return result;
  }

}