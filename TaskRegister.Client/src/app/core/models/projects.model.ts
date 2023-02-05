export interface CreateProject {
    projectName: string;
}

export interface ProjectDto {
    id: string;
    subject: string;
    projectName: string;
    active: boolean;
}