import { Utility } from "./Utility";

export interface ProjectCreationData {
    name: string,
    description: string
}

export class Project implements ProjectCreationData {

    static async create(project: ProjectCreationData): Promise<Project> {
        const response = await fetch("api/Project", {
            method: "POST",
            body: JSON.stringify(project),
            headers: {
                "Content-Type": "application/json"
            }
        });

        Utility.checkResponse(response);

        return await response.json();
    }

    id: number;
    name: string;
    description: string;

    constructor(id: number, name: string, description: string) {
        this.id = id;
        this.name = name;
        this.description = description;
    }
}