import { useEffect, useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Button, Dialog, DialogTitle, DialogContent, DialogActions, TextField, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import BarChartIcon from '@mui/icons-material/BarChart';
import axiosInstance from '../api/axiosInstance';
import { AuthContext } from '../context/AuthContext';
import { type CandidateResponseDto, type CandidateCreateRequestDto } from '../types/apiTypes';

interface CandidateFormState {
    fullName: string;
    dateOfBirth: string;
    expectedSalary: string;
    experienceYears: string;
    techStack: string;
    contactInfo: string;
}

const initialFormState: CandidateFormState = {
    fullName: '', dateOfBirth: '', expectedSalary: '', experienceYears: '', techStack: '', contactInfo: ''
};

export default function CandidatesPage() {
    const [candidates, setCandidates] = useState<CandidateResponseDto[]>([]);
    const [open, setOpen] = useState<boolean>(false);
    const [form, setForm] = useState<CandidateFormState>(initialFormState);
    const { user } = useContext(AuthContext);
    const navigate = useNavigate();

    const fetchCandidates = async () => {
        const res = await axiosInstance.get<CandidateResponseDto[]>('/candidates');
        setCandidates(res.data);
    };

    useEffect(() => { fetchCandidates(); }, []);

    const handleCreate = async () => {
        const payload: CandidateCreateRequestDto = {
            fullName: form.fullName,
            dateOfBirth: form.dateOfBirth,
            expectedSalary: Number(form.expectedSalary),
            experienceYears: Number(form.experienceYears),
            techStack: form.techStack,
            contactInfo: form.contactInfo
        };

        await axiosInstance.post('/candidates', payload);
        setOpen(false);
        setForm(initialFormState);
        fetchCandidates();
    };

    const handleFormChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    return (
        <Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
                <Typography variant="h4">Кандидаты</Typography>
                <Box>
                    <Button variant="contained" startIcon={<AddIcon />} onClick={() => setOpen(true)} sx={{ mr: 2 }}>
                        Добавить
                    </Button>
                    <Button variant="outlined" startIcon={<BarChartIcon />} onClick={() => navigate('/analytics')}>
                        Аналитика
                    </Button>
                </Box>
            </Box>

            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>ФИО</TableCell>
                            <TableCell>Дата рождения</TableCell>
                            <TableCell>Желаемая ЗП</TableCell>
                            <TableCell>Опыт (лет)</TableCell>
                            <TableCell>Стек</TableCell>
                            <TableCell>Статус</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {candidates.map((c) => (
                            <TableRow key={c.id}>
                                <TableCell>{c.fullName}</TableCell>
                                <TableCell>{new Date(c.dateOfBirth).toLocaleDateString()}</TableCell>
                                <TableCell>{c.expectedSalary.toLocaleString()} ₽</TableCell>
                                <TableCell>{c.experienceYears}</TableCell>
                                <TableCell>{c.techStack}</TableCell>
                                <TableCell>{c.status}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>

            <Dialog open={open} onClose={() => setOpen(false)}>
                <DialogTitle>Новый кандидат</DialogTitle>
                <DialogContent sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 1, width: 400 }}>
                    <TextField name="fullName" label="ФИО" value={form.fullName} onChange={handleFormChange} required />
                    <TextField name="dateOfBirth" label="Дата рождения" type="date" InputLabelProps={{ shrink: true }} value={form.dateOfBirth} onChange={handleFormChange} required />
                    <TextField name="expectedSalary" label="Желаемая ЗП" type="number" value={form.expectedSalary} onChange={handleFormChange} required />
                    <TextField name="experienceYears" label="Опыт (лет)" type="number" value={form.experienceYears} onChange={handleFormChange} required />
                    <TextField name="techStack" label="Стек технологий" value={form.techStack} onChange={handleFormChange} required />
                    <TextField name="contactInfo" label="Контактная информация" value={form.contactInfo} onChange={handleFormChange} required />
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setOpen(false)}>Отмена</Button>
                    <Button onClick={handleCreate} variant="contained">Сохранить</Button>
                </DialogActions>
            </Dialog>
        </Box>
    );
}