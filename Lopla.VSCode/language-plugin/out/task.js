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
        let tasks = [];
        let d = {
            type: "lopla"
        };
        tasks.push(this.getTask(d));
        return tasks;
    }
    getTask(definition) {
        const def = definition;
        let startingFolder = this._workspaceRoot;
        if (def && def.folder) {
            startingFolder = def.folder.toString();
        }
        return new vscode.Task(definition, vscode_1.TaskScope.Workspace, "run", "lopla", new vscode.ShellExecution(intelisense_1.loplaTool, [startingFolder]));
    }
    resolveTask(_task, token) {
        return this.getTask(_task.definition);
    }
}
exports.LoplaTaskProvider = LoplaTaskProvider;
//# sourceMappingURL=task.js.map