import { render, screen } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import { AuthProvider } from '../auth/AuthContext';
import {{Name}}Page from './{{Name}}Page';
import { describe, it, expect, vi } from 'vitest';

// Mock the hook
vi.mock('../hooks/use{{Name}}', () => ({
  default: () => ({
    items: [{ id: '1', name: 'Mock {{Name}}' }],
    loading: false
  })
}));

describe('{{Name}}Page', () => {
  it('renders table with data', () => {
    render(
      <BrowserRouter>
        <AuthProvider>
          <{{Name}}Page />
        </AuthProvider>
      </BrowserRouter>
    );

    expect(screen.getByRole('heading', { name: /{{Name}}/i })).toBeInTheDocument();
    expect(screen.getByText(/Mock {{Name}}/i)).toBeInTheDocument();
  });
});
