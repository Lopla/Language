import * as vscode from 'vscode';
import { CompletionItemProvider } from "vscode";
import { LoplaSchema } from "./intelisense";

export class LibsCompletetionProvider implements CompletionItemProvider{
    provideCompletionItems(document: import("vscode").TextDocument, position: import("vscode").Position, token: import("vscode").CancellationToken, context: import("vscode").CompletionContext): import("vscode").ProviderResult<import("vscode").CompletionItem[] | import("vscode").CompletionList> {
        
        let p = position.translate(0, -position.character);
        let line = document.getText(new vscode.Range(p, position));

        for (const key of Object.keys(LoplaSchema)) {
          let rs = '\\b' + key +'\\.';
          let r= new RegExp(rs);
          
          if(line && r.test(line)){
            var suggestions = [];
            
            var methods = Object.keys(LoplaSchema[key]);
            methods.forEach(element => {
              suggestions.push(new vscode.CompletionItem(element, vscode.CompletionItemKind.Function));
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