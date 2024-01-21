import React, { useState, useEffect } from 'react';
import axios from 'axios';

import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import Pagination from '@mui/material/Pagination';
import Button from '@mui/material/Button';
import CircularProgress from '@mui/material/CircularProgress';

import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import BasicModal from '../ModalWindow/ModalWIndow';
import { blue } from '@mui/material/colors';

const Grid = () =>{
    const [open, setOpen] = React.useState(false);
    const handleOpen = () => setOpen(true);
    const handleClose = () => setOpen(false);

    const [loading, setLoading] = useState(false);
    const [urlPage, setUrlPage] = useState({ items: [], totalCount: 0 });
    const [currentPage, setCurrentPage] = useState(1);
    const [userInfo, setUserInfo] = useState({ id: null, role: null });
    const recordsPerPage = 5;
    const numberOfPages = Math.ceil(urlPage.totalCount / recordsPerPage);
    const token = localStorage.getItem('token');
    
    useEffect(() => {
        const fetchUserInfo = async () => {
            if (token) {
                try {
                    const endpoint = `https://localhost:7136/user/claims`;
                    const response = await axios.get(
                        endpoint,
                        { headers: { Authorization: `Bearer ${token}`} }
                    );

                    console.log(response.data);
                    const { id, role } = response.data;

                    console.log('User ID:', id);
                    console.log('User Role:', role);

                    setUserInfo({ id, role });
                } catch (error) {
                    console.error('Error decoding JWT:', error.message);
                }
            }
        };

        fetchUserInfo();
    }, []);

    useEffect(() => {
        const endpoint = `https://localhost:7136/url/page?page=${currentPage}`;
        axios.get(endpoint)
          .then(response => {
            console.log(response.data);
            setUrlPage(response.data);
          })
          .catch(error => {
            console.error('Error fetching URL page:', error);
          });
      }, [currentPage]);

    const fetchData = async (page) => {
        const endpoint = `https://localhost:7136/url/page?page=${page}`;
        try {
            const response = await axios.get(endpoint);
            setUrlPage(response.data);
        } catch (error) {
            console.error('Error fetching URL page:', error);
        }
    };

    const handleAddUrl = async () => {
        const userUrl = prompt('Enter the URL for shortening:');
        if (userUrl === null || userUrl.trim() === '') {
            toast('Please enter a valid URL.');
            return;
        }
        setLoading(true);
        try {
            const endpoint = `https://localhost:7136/url/adding`;

            const response = await axios.post(
                endpoint,
                `"${userUrl}"`,
                { headers: { Authorization: `Bearer ${token}`, 'Content-Type': 'application/json' } }
            );
            toast(response.data);
            fetchData(currentPage);
        } catch (error) {
            toast(error.response.data)
        } finally {
            setLoading(false);
        }
    };

    const canDelete = (url) => {
        if (token) {
            try {
                return url.userId === userInfo.id || userInfo.role === "admin";
            } catch (error) {
                console.error('Error decoding JWT:', error.message);
            }
        }
        return false;
    };

    const onDeleteRow = async (rowId) => {
        console.log(`Deleting row with ID: ${rowId}`);
        const endpoint = `https://localhost:7136/url/${rowId}`;

        try {
            const response = await axios.delete(
                endpoint,
                { headers: { Authorization: `Bearer ${token}`, 'Content-Type': 'application/json' } }
            );
            if (response.status === 200) {
                console.log(response.data);
                toast('URL was deleted.');
                fetchData(currentPage);
            }
        }
        catch(error){
            toast(error.response.data);
        }
    };

    
    return(
        
        <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center',
        maxWidth: '800px', margin: '0 auto', padding: '20px', }}>
            {loading 
            ? (
                <CircularProgress style={{ marginLeft: '8px' }} />
            )
            : (
                <>
                    {urlPage.items.length > 0 ? (
                        <TableContainer component={Paper}>
                            <Table sx={{ minWidth: 650 }} aria-label="simple table">
                                <TableHead>
                                    <TableRow>
                                        <TableCell>Id</TableCell>
                                        <TableCell align="right">Long url</TableCell>
                                        <TableCell align="right">Short url</TableCell>
                                        {userInfo.id !== null && (
                                            <TableCell align="right">Actions</TableCell>
                                        )}

                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {urlPage.items.map((url) => (
                                        <TableRow key={url.id} sx={{ '&:last-child td, &:last-child th': { border: 0 } }}>
                                            <TableCell style={{ color: userInfo.id !== null ? 'blue' : 'inherit' }} onClick={() => userInfo.id !== null && handleOpen()} component="th" scope="row">
                                                {url.id}
                                            </TableCell>
                                            <TableCell align="right">{url.longUrl}</TableCell>
                                            <TableCell align="right">{url.shortUrl}</TableCell>
                                            {canDelete(url) ? (
                                                <TableCell align="right">
                                                    <Button onClick={() => onDeleteRow(url.id)} variant="contained" color="error">
                                                        Delete
                                                    </Button>
                                                </TableCell>
                                            )
                                            :(
                                                <TableCell align="right">No Actions</TableCell>
                                            )}
                                            <BasicModal open={open} handleClose={handleClose} url={url}/>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </TableContainer>
                    ) : (
                        <p>No items to display.</p>
                    )}

                    {numberOfPages > 0 && (
                        <Pagination count={numberOfPages} page={currentPage} onChange={(event, value) => setCurrentPage(value)} />
                    )}

                    {userInfo.id !== null && (
                        <Button onClick={handleAddUrl} variant="outlined">
                            Add URL for Shortening
                        </Button>
                    )}
                    
                </>
            )}
        </div>
    )
}
export default Grid;
