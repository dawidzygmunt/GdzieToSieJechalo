import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@/test/utils';
import userEvent from '@testing-library/user-event';
import { AddKierowcaDialog } from './AddKierowcaDialog';
import * as useKierowcyModule from '@/hooks/useKierowcy';

// Mock the useCreateKierowca hook
vi.mock('@/hooks/useKierowcy', () => ({
  useCreateKierowca: vi.fn(),
}));

describe('AddKierowcaDialog', () => {
  const mockMutateAsync = vi.fn();
  const mockOnOpenChange = vi.fn();

  beforeEach(() => {
    vi.clearAllMocks();
    vi.mocked(useKierowcyModule.useCreateKierowca).mockReturnValue({
      mutateAsync: mockMutateAsync,
      isPending: false,
      isError: false,
      isSuccess: false,
      error: null,
      data: undefined,
      mutate: vi.fn(),
      reset: vi.fn(),
      context: undefined,
      failureCount: 0,
      failureReason: null,
      isPaused: false,
      status: 'idle',
      submittedAt: 0,
      variables: undefined,
    } as any);
  });

  it('should render the dialog when open', () => {
    render(<AddKierowcaDialog open={true} onOpenChange={mockOnOpenChange} />);

    expect(screen.getByText('Dodaj nowego kierowcę')).toBeInTheDocument();
    expect(screen.getByLabelText(/Imię/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Nazwisko/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Numer pracownika/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Data zatrudnienia/i)).toBeInTheDocument();
  });

  it('should not render the dialog when closed', () => {
    render(<AddKierowcaDialog open={false} onOpenChange={mockOnOpenChange} />);

    expect(screen.queryByText('Dodaj nowego kierowcę')).not.toBeInTheDocument();
  });

  it('should show validation errors for empty fields', async () => {
    const user = userEvent.setup();
    render(<AddKierowcaDialog open={true} onOpenChange={mockOnOpenChange} />);

    const submitButton = screen.getByRole('button', { name: /Dodaj kierowcę/i });
    await user.click(submitButton);

    await waitFor(() => {
      expect(screen.getByText('Imię jest wymagane')).toBeInTheDocument();
      expect(screen.getByText('Nazwisko jest wymagane')).toBeInTheDocument();
      expect(screen.getByText('Numer pracownika jest wymagany')).toBeInTheDocument();
      expect(screen.getByText('Data zatrudnienia jest wymagana')).toBeInTheDocument();
    });

    expect(mockMutateAsync).not.toHaveBeenCalled();
  });

  it('should submit form with valid data', async () => {
    const user = userEvent.setup();
    mockMutateAsync.mockResolvedValue(1);

    render(<AddKierowcaDialog open={true} onOpenChange={mockOnOpenChange} />);

    const imieInput = screen.getByLabelText(/Imię/i);
    const nazwiskoInput = screen.getByLabelText(/Nazwisko/i);
    const nrPracownikaInput = screen.getByLabelText(/Numer pracownika/i);
    const dataZatrudnieniaInput = screen.getByLabelText(/Data zatrudnienia/i);

    await user.type(imieInput, 'Jan');
    await user.type(nazwiskoInput, 'Kowalski');
    await user.type(nrPracownikaInput, '12345');
    await user.type(dataZatrudnieniaInput, '2024-01-15');

    const submitButton = screen.getByRole('button', { name: /Dodaj kierowcę/i });
    await user.click(submitButton);

    await waitFor(() => {
      expect(mockMutateAsync).toHaveBeenCalledWith({
        imie: 'Jan',
        nazwisko: 'Kowalski',
        nrPracownika: '12345',
        dataZatrudnienia: '2024-01-15',
      });
    });

    await waitFor(() => {
      expect(mockOnOpenChange).toHaveBeenCalledWith(false);
    });
  });

  it('should call onOpenChange when cancel button is clicked', async () => {
    const user = userEvent.setup();
    render(<AddKierowcaDialog open={true} onOpenChange={mockOnOpenChange} />);

    const cancelButton = screen.getByRole('button', { name: /Anuluj/i });
    await user.click(cancelButton);

    expect(mockOnOpenChange).toHaveBeenCalledWith(false);
  });

  it('should disable buttons when submitting', () => {
    vi.mocked(useKierowcyModule.useCreateKierowca).mockReturnValue({
      mutateAsync: mockMutateAsync,
      isPending: true,
      isError: false,
      isSuccess: false,
      error: null,
      data: undefined,
      mutate: vi.fn(),
      reset: vi.fn(),
      context: undefined,
      failureCount: 0,
      failureReason: null,
      isPaused: false,
      status: 'pending',
      submittedAt: 0,
      variables: undefined,
    } as any);

    render(<AddKierowcaDialog open={true} onOpenChange={mockOnOpenChange} />);

    const submitButton = screen.getByRole('button', { name: /Dodaj kierowcę/i });
    const cancelButton = screen.getByRole('button', { name: /Anuluj/i });

    expect(submitButton).toBeDisabled();
    expect(cancelButton).toBeDisabled();
  });

  it('should handle API errors gracefully', async () => {
    const user = userEvent.setup();
    const consoleErrorSpy = vi.spyOn(console, 'error').mockImplementation(() => {});
    mockMutateAsync.mockRejectedValue(new Error('API Error'));

    render(<AddKierowcaDialog open={true} onOpenChange={mockOnOpenChange} />);

    const imieInput = screen.getByLabelText(/Imię/i);
    const nazwiskoInput = screen.getByLabelText(/Nazwisko/i);
    const nrPracownikaInput = screen.getByLabelText(/Numer pracownika/i);
    const dataZatrudnieniaInput = screen.getByLabelText(/Data zatrudnienia/i);

    await user.type(imieInput, 'Jan');
    await user.type(nazwiskoInput, 'Kowalski');
    await user.type(nrPracownikaInput, '12345');
    await user.type(dataZatrudnieniaInput, '2024-01-15');

    const submitButton = screen.getByRole('button', { name: /Dodaj kierowcę/i });
    await user.click(submitButton);

    await waitFor(() => {
      expect(consoleErrorSpy).toHaveBeenCalledWith(
        '[AddKierowcaDialog] Błąd podczas tworzenia kierowcy:',
        expect.any(Error)
      );
    });

    // Check that alert was called
    expect(global.alert).toHaveBeenCalled();

    expect(mockOnOpenChange).not.toHaveBeenCalled();
    consoleErrorSpy.mockRestore();
  });
});
