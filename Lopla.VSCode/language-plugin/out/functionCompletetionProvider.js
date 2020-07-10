"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const vscode = require("vscode");
const intelisense_1 = require("./intelisense");
const vscode_1 = require("vscode");
class FunctionCompletetionProvider {
    provideSignatureHelp(document, position, token, context) {
        var result = new vscode_1.SignatureHelp();
        let p = position.translate(0, -position.character);
        let line = document.getText(new vscode.Range(p, position));
        for (var key of Object.keys(intelisense_1.LoplaMethods)) {
            let rs = (key).replace(".", "[.]").toString();
            let r = new RegExp(rs);
            if (line && r.test(line)) {
                var mdString = "";
                var methods = Object.values(intelisense_1.LoplaMethods[key]);
                methods.forEach(element => {
                    mdString = mdString + "* " + element.toString().trim() + "\n";
                });
                var signature = new vscode_1.SignatureInformation(key, new vscode.MarkdownString(mdString));
                result.signatures.push(signature);
                return result;
            }
        }
        return result;
    }
}
exports.FunctionCompletetionProvider = FunctionCompletetionProvider;
//# sourceMappingURL=functionCompletetionProvider.js.map