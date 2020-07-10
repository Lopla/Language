import * as vscode from 'vscode';
import { CompletionItemProvider } from "vscode";
import { LoplaMethods } from "./intelisense";

export class FunctionCompletetionProvider implements CompletionItemProvider{
    provideCompletionItems(document: import("vscode").TextDocument, 
    position: import("vscode").Position, 
    token: import("vscode").CancellationToken, 
    context: import("vscode").CompletionContext): import("vscode").ProviderResult<import("vscode").CompletionItem[] | import("vscode").CompletionList> {
        
        let p = position.translate(0, -position.character);
        let line = document.getText(new vscode.Range(p, position));

        console.log(LoplaMethods);

        for (const key of Object.keys(LoplaMethods)) {
          let rs =  key +'\\(';
          let r= new RegExp(rs, "g");
          
          if( line && r.test(line)){
            console.log(line);
            var suggestions = [];
            
            var methods = Object.keys(LoplaMethods[key]);
            methods.forEach(element => {
              suggestions.push(
                new vscode.CompletionItem(element, vscode.CompletionItemKind.TypeParameter));
            })

            return suggestions;
          }
        }
        
        return null;
    }
    
    resolveCompletionItem?(item: import("vscode").CompletionItem, token: import("vscode").CancellationToken): import("vscode").ProviderResult<import("vscode").CompletionItem> {
        throw new Error("Method not implemented.");
    }


}