"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const path = require("path");
const fs = require("fs");
const vscode = require("vscode");
class RunTaskProvider {
    constructor(workspaceRoot) {
        this.rakePromise = undefined;
        let pattern = path.join(workspaceRoot, 'Rakefile');
        let fileWatcher = vscode.workspace.createFileSystemWatcher(pattern);
        fileWatcher.onDidChange(() => this.rakePromise = undefined);
        fileWatcher.onDidCreate(() => this.rakePromise = undefined);
        fileWatcher.onDidDelete(() => this.rakePromise = undefined);
    }
    provideTasks() {
        // if (!this.rakePromise) {
        // 	this.rakePromise = getRakeTasks();
        // }
        return this.rakePromise;
    }
    resolveTask(_task) {
        const task = _task.definition.task;
        // A Rake task consists of a task and an optional file as specified in RakeTaskDefinition
        // Make sure that this looks like a Rake task by checking that there is a task.
        if (task) {
            // resolveTask requires that the same definition object be used.
            const definition = _task.definition;
            return new vscode.Task(definition, definition.task, 'rake', new vscode.ShellExecution(`rake ${definition.task}`));
        }
        return undefined;
    }
}
RunTaskProvider.LoplaType = 'lopla';
exports.RunTaskProvider = RunTaskProvider;
function exists(file) {
    return new Promise((resolve, _reject) => {
        fs.exists(file, (value) => {
            resolve(value);
        });
    });
}
//# sourceMappingURL=runTaskProvider.js.map