"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const vscode = require("vscode");
const intelisense_1 = require("./intelisense");
class FunctionCompletetionProvider {
    provideCompletionItems(document, position, token, context) {
        let p = position.translate(0, -position.character);
        let line = document.getText(new vscode.Range(p, position));
        console.log(intelisense_1.LoplaMethods);
        for (const key of Object.keys(intelisense_1.LoplaMethods)) {
            let rs = key + '[(]';
            let r = new RegExp(rs, "g");
            if (line && r.test(line)) {
                console.log(line);
                var suggestions = [];
                var methods = Object.keys(intelisense_1.LoplaMethods[key]);
                methods.forEach(element => {
                    suggestions.push(new vscode.CompletionItem(element, vscode.CompletionItemKind.TypeParameter));
                });
                return suggestions;
            }
            else {
                console.log("Nope: ", rs, line);
            }
        }
        return null;
    }
    resolveCompletionItem(item, token) {
        throw new Error("Method not implemented.");
    }
}
exports.FunctionCompletetionProvider = FunctionCompletetionProvider;
//# sourceMappingURL=functionCompletetionProvider.js.map