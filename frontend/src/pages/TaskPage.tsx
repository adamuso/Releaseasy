import * as React from "react";
import { Page } from "./Page";
import { Project } from "../backend/Project";
import { User } from "../backend/User";
import { Task } from "../backend/Task";
import { EditableMarkdownDisplay } from "../components/EditableMarkdownDisplay";
import { Application } from "../main";

export class TaskPage extends Page<{ sidebarOpen: boolean, project?: Project, creator?: any, task?: Task }, { projectId: number, id: number }> {
    constructor(props) {
        super(props);

        this.state = {
            sidebarOpen: false,
        };
    }

    componentDidMount() {
        Project.get(this.params.projectId!.toString()).then(project => {
            this.setState({ project });
        });

        Task.get(this.params.id!).then(task => {
            this.setState({ task });

            User.get(task.creator).then(creator => {
                this.setState({ creator });
            });
        });
    }

    onSetSidebarOpen(open: boolean) {
    }

    render() {
        return <div className="task-page">
            <div className="task-path"><a onClick={() => {
                Application.reactApp.changePage(`Project?id=${this.state.project?.id}`);
            }}>{this.state.project?.name}</a> 🠢 {this.state.task?.name}</div>
            <div className="task-title">Task: {this.state.task?.name}</div>
            <div className="task-info">
                <div className="task-description">
                    {this.state.task ? <EditableMarkdownDisplay value={(value?: string) => {
                        if (value === undefined) {
                            return this.state.task!.description;
                        }

                        Task.edit(this.params.id!, { description: value });
                        this.state.task!.description = value;
                        this.forceUpdate();

                        return this.state.task!.description;
                    }}/> : null}
                </div>
                <div className="task-tags">
                    <div className="task-tags-title">Tags</div>
                </div>
                <div className="task-date">
                    <div className="task-date-title">Creation date</div>
                    <div className="task-date-value">{this.state.task ? new Date(Date.parse(this.state.task.startTime)).toLocaleString() : null}</div>
                </div>
            </div>
            <div>Assigned</div>

            {this.params.projectId} {this.params.id}
        </div>;
    }
}
