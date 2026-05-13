import { describe, it, expect, vi } from 'vitest';
import { render, screen, waitFor } from '@/test/utils';
import userEvent from '@testing-library/user-event';
import { AddKierowcaDialog } from './AddKierowcaDialog';
import { http, HttpResponse } from 'msw';
import { server } from '@/test/mocks/server';
import { API_BASE_URL } from '@/lib/constants';

describe('AddKierowcaDialog Integration', () => {
  it('should successfully create a kierowca with valid data', async () => {
    const user = userEvent.setup();
    const mockOnOpenChange = vi.fn();

    render(<AddKierowcaDialog open={true} onOpenChange={mockOnOpenChange} />);

    // Fill in the form
    await user.type(screen.getByLabelText(/Imię/i), 'Jan');
    await user.type(screen.getByLabelText(/Nazwisko/i), 'Kowalski');
    await user.type(screen.getByLabelText(/Numer pracownika/i), '12345');
    await user.type(screen.getByLabelText(/Data zatrudnienia/i), '2024-01-15');

    // Submit the form
    const submitButton = screen.getByRole('button', { name: /Dodaj kierowcę/i });
    await user.click(submitButton);

    // Wait for the dialog to close
    await waitFor(() => {
      expect(mockOnOpenChange).toHaveBeenCalledWith(false);
    }, { timeout: 3000 });
  });

  it('should handle API validation errors', async () => {
    const user = userEvent.setup();
    const mockOnOpenChange = vi.fn();

    // Override the handler to return validation error
    server.use(
      http.post(`${API_BASE_URL}/kierowcy`, () => {
        return HttpResponse.json(
          {
            type: 'ValidationError',
            title: 'One or more validation errors occurred.',
            status: 400,
            errors: {
              NrPracownika: ['Numer pracownika już istnieje.'],
            },
          },
          { status: 400 }
        );
      })
    );

    // Mock window.alert
    const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});

    render(<AddKierowcaDialog open={true} onOpenChange={mockOnOpenChange} />);

    // Fill in the form
    await user.type(screen.getByLabelText(/Imię/i), 'Jan');
    await user.type(screen.getByLabelText(/Nazwisko/i), 'Kowalski');
    await user.type(screen.getByLabelText(/Numer pracownika/i), '12345');
    await user.type(screen.getByLabelText(/Data zatrudnienia/i), '2024-01-15');

    // Submit the form
    const submitButton = screen.getByRole('button', { name: /Dodaj kierowcę/i });
    await user.click(submitButton);

    // Wait for error alert
    await waitFor(() => {
      expect(alertSpy).toHaveBeenCalled();
    }, { timeout: 3000 });

    // Dialog should NOT close on error
    expect(mockOnOpenChange).not.toHaveBeenCalledWith(false);

    alertSpy.mockRestore();
  });

  it('should handle network errors', async () => {
    const user = userEvent.setup();
    const mockOnOpenChange = vi.fn();

    // Override the handler to simulate network error
    server.use(
      http.post(`${API_BASE_URL}/kierowcy`, () => {
        return HttpResponse.error();
      })
    );

    // Mock window.alert
    const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});

    render(<AddKierowcaDialog open={true} onOpenChange={mockOnOpenChange} />);

    // Fill in the form
    await user.type(screen.getByLabelText(/Imię/i), 'Jan');
    await user.type(screen.getByLabelText(/Nazwisko/i), 'Kowalski');
    await user.type(screen.getByLabelText(/Numer pracownika/i), '12345');
    await user.type(screen.getByLabelText(/Data zatrudnienia/i), '2024-01-15');

    // Submit the form
    const submitButton = screen.getByRole('button', { name: /Dodaj kierowcę/i });
    await user.click(submitButton);

    // Wait for error alert
    await waitFor(() => {
      expect(alertSpy).toHaveBeenCalled();
    }, { timeout: 3000 });

    // Dialog should NOT close on error
    expect(mockOnOpenChange).not.toHaveBeenCalledWith(false);

    alertSpy.mockRestore();
  });

  it('should send data in correct format to API', async () => {
    const user = userEvent.setup();
    const mockOnOpenChange = vi.fn();
    let capturedRequestBody: any = null;

    // Override the handler to capture request
    server.use(
      http.post(`${API_BASE_URL}/kierowcy`, async ({ request }) => {
        capturedRequestBody = await request.json();
        return HttpResponse.json(1, { status: 201 });
      })
    );

    render(<AddKierowcaDialog open={true} onOpenChange={mockOnOpenChange} />);

    // Fill in the form
    await user.type(screen.getByLabelText(/Imię/i), 'Jan');
    await user.type(screen.getByLabelText(/Nazwisko/i), 'Kowalski');
    await user.type(screen.getByLabelText(/Numer pracownika/i), '12345');
    await user.type(screen.getByLabelText(/Data zatrudnienia/i), '2024-01-15');

    // Submit the form
    const submitButton = screen.getByRole('button', { name: /Dodaj kierowcę/i });
    await user.click(submitButton);

    // Wait for request to be captured
    await waitFor(() => {
      expect(capturedRequestBody).not.toBeNull();
    }, { timeout: 3000 });

    // Verify the data format
    expect(capturedRequestBody).toEqual({
      imie: 'Jan',
      nazwisko: 'Kowalski',
      nrPracownika: '12345',
      dataZatrudnienia: '2024-01-15',
    });

    // Verify camelCase property names
    expect(capturedRequestBody).toHaveProperty('imie');
    expect(capturedRequestBody).toHaveProperty('nazwisko');
    expect(capturedRequestBody).toHaveProperty('nrPracownika');
    expect(capturedRequestBody).toHaveProperty('dataZatrudnienia');

    // Verify date format
    expect(capturedRequestBody.dataZatrudnienia).toMatch(/^\d{4}-\d{2}-\d{2}$/);
  });
});
