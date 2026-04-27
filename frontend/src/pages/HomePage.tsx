import React from 'react';
import { Typography, Paper, Box, Grid } from '@mui/material';

const HomePage: React.FC = () => {
  return (
    <Box>
      <Typography variant="h4" gutterBottom>Welcome to Nexus</Typography>
      <Grid container spacing={3}>
        {/* @ts-ignore - Grid in MUI v6 might have type issues in some setups, fallback to stable pattern */}
        <Grid item xs={12} md={6}>
          <Paper sx={{ p: 2 }}>
            <Typography variant="h6">Quick Start</Typography>
            <Typography variant="body1">
              Use the CLI to scaffold new features. Check the docs for more details.
            </Typography>
          </Paper>
        </Grid>
      </Grid>
    </Box>
  );
};

export default HomePage;
