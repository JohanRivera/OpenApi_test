export interface TaskRegisterDto {
    subject: string;
    subjectUserId: string;
    projectId: string;
    projectName: string;
    assignedTo: string;
    taskDescription: string;
    comment: string;
    time: number;
    assignedDate: string;
}

export interface SearchTimeslipDto {
    subjectUserId: string;
    projectId: string;
    isAll: boolean;
    dateLimitUp: string;
    dateLimitDown: string;
}

export interface DeleteTimeslipDto {
    subject: string;
}

export interface TimelipsDto {
    subject: string;
    subjectUserId: string;
    projectId: string;
    projectName: string;
    assignedTo: string;
    taskDescription: string;
    comment: string;
    time: string;
    assignedDate: string;
}