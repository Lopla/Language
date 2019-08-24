"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const vscode = require("vscode");
class LoplaTaskProvider {
    provideTasks(token) {
        if (this.tasks == undefined) {
            this.tasks = [];
            this.tasks.push(this.getTask());
        }
        return this.tasks;
    }
    getTask() {
        const definition = null;
        return new vscode.Task(definition, definition.task, 'lopla', new vscode.ShellExecution(`lopla ${definition.task}`));
    }
    resolveTask(_task, token) {
        const task = _task.definition.task;
        // A Rake task consists of a task and an optional file as specified in RakeTaskDefinition
        // Make sure that this looks like a Rake task by checking that there is a task.
        if (task) {
            // resolveTask requires that the same definition object be used.
            const definition = _task.definition;
            return new vscode.Task(definition, definition.task, 'lopla', new vscode.ShellExecution(`lopla ${definition.task}`));
        }
        return undefined;
    }
}
exports.LoplaTaskProvider = LoplaTaskProvider;
//# sourceMappingURL=task.js.map