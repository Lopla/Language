import * as vscode from 'vscode';
import { CompletionItemProvider, CompletionItem, CancellationToken, ProviderResult, TextDocument, Position, CompletionContext, CompletionList } from "vscode";
import { LoplaSchema } from "./intelisense";

export class LibsCompletetionProvider implements CompletionItemProvider{
    provideCompletionItems(document: TextDocument, position: Position, token: CancellationToken, context: CompletionContext): ProviderResult<CompletionItem[] | CompletionList> {
        
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
    
    resolveCompletionItem?(item: CompletionItem, token: CancellationToken): ProviderResult<CompletionItem> {
        throw new Error("Method not implemented.");
    }


}