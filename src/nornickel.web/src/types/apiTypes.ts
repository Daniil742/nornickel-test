export enum UserRoleDto {
    Admin = 'Admin',
    HR = 'HR',
    Interviewer = 'Interviewer'
}

export enum CandidateStatus {
    New = 'New',
    Screening = 'Screening',
    Interview = 'Interview',
    Offer = 'Offer',
    Hired = 'Hired',
    Rejected = 'Rejected'
}

export interface LoginRequestDto {
    username: string;
    password: string;
}

export interface LoginResponseDto {
    message: string;
    username: string;
    role: UserRoleDto;
    token: string;
}

export interface CandidateCreateRequestDto {
    fullName: string;
    dateOfBirth: string;
    expectedSalary: number;
    experienceYears: number;
    techStack: string;
    contactInfo: string;
}

export interface CandidateResponseDto {
    id: number;
    fullName: string;
    dateOfBirth: string;
    expectedSalary: number;
    experienceYears: number;
    techStack: string;
    contactInfo: string;
    status: CandidateStatus;
    createdAt: string;
}

export interface ChartDataResponseDto {
    labels: string[];
    values: number[];
}