import React from 'react';
import { Typography, Box, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Button } from '@mui/material';
import use{{Name}} from '../hooks/use{{Name}}';

const {{Name}}Page: React.FC = () => {
  const { items, loading, createItem } = use{{Name}}();

  const handleCreate = async () => {
    // Generate a generic name/status for the new item. 
    // In a real app, this might come from a form or dialog.
    const newName = `New {{Name}} ${new Date().toLocaleTimeString()}`;
    await createItem(newName);
  };

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
        <Typography variant="h4" gutterBottom>{{Name}}</Typography>
        <Button variant="contained" color="primary" onClick={handleCreate}>
          Create {{Name}}
        </Button>
      </Box>
      
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Name</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {items.map((item) => (
              <TableRow key={item.id}>
                <TableCell>{item.id}</TableCell>
                <TableCell>{item.name}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
};

export default {{Name}}Page;
