"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const vscode = require("vscode");
const vscode_1 = require("vscode");
const intelisense_1 = require("./intelisense");
class LoplaTaskProvider {
    constructor(_workspaceRoot) {
        this._workspaceRoot = _workspaceRoot;
    }
    provideTasks(token) {
        // if(this.tasks == undefined)
        // {
        //     this.tasks = []; 
        //     let def = {}
        //     this.tasks.push(this.getTask({}));
        // }
        return undefined;
    }
    getTask(definition) {
        return new vscode.Task(definition, vscode_1.TaskScope.Workspace, "run", "lopla", new vscode.ShellExecution(intelisense_1.loplaTool, {
            shellArgs: [this._workspaceRoot]
        }));
    }
    resolveTask(_task, token) {
        return this.getTask(_task.definition);
    }
}
exports.LoplaTaskProvider = LoplaTaskProvider;
//# sourceMappingURL=task.js.map