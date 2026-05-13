import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogDescription,
  DialogFooter,
} from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { useCreateKierowca } from '@/hooks/useKierowcy';
import type { UtworzKierowceDto } from '@/types/kierowca.types';

const kierowcaSchema = z.object({
  imie: z.string().min(1, 'Imię jest wymagane'),
  nazwisko: z.string().min(1, 'Nazwisko jest wymagane'),
  nrPracownika: z.string().min(1, 'Numer pracownika jest wymagany'),
  dataZatrudnienia: z.string().min(1, 'Data zatrudnienia jest wymagana'),
});

type KierowcaFormData = z.infer<typeof kierowcaSchema>;

interface AddKierowcaDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

export function AddKierowcaDialog({ open, onOpenChange }: AddKierowcaDialogProps) {
  const createKierowca = useCreateKierowca();

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<KierowcaFormData>({
    resolver: zodResolver(kierowcaSchema),
  });

  const onSubmit = async (data: KierowcaFormData) => {
    try {
      const payload: UtworzKierowceDto = {
        imie: data.imie,
        nazwisko: data.nazwisko,
        nrPracownika: data.nrPracownika,
        dataZatrudnienia: data.dataZatrudnienia,
      };

      console.log('[AddKierowcaDialog] Wysyłanie danych:', payload);
      await createKierowca.mutateAsync(payload);
      console.log('[AddKierowcaDialog] Sukces!');
      reset();
      onOpenChange(false);
    } catch (error) {
      console.error('[AddKierowcaDialog] Błąd podczas tworzenia kierowcy:', error);
      if (error instanceof Error) {
        console.error('[AddKierowcaDialog] Szczegóły błędu:', error.message);
      }
      // Show error to user
      alert(`Błąd podczas dodawania kierowcy: ${error instanceof Error ? error.message : 'Nieznany błąd'}`);
    }
  };

  const handleClose = () => {
    reset();
    onOpenChange(false);
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent onClose={handleClose}>
        <DialogHeader>
          <DialogTitle>Dodaj nowego kierowcę</DialogTitle>
          <DialogDescription>
            Wypełnij formularz, aby dodać nowego kierowcę do systemu
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="space-y-4">
            <div>
              <Label htmlFor="imie" required>
                Imię
              </Label>
              <Input
                id="imie"
                {...register('imie')}
                error={!!errors.imie}
                placeholder="np. Jan"
                className="mt-1.5"
              />
              {errors.imie && (
                <p className="text-sm text-red-500 mt-1">
                  {errors.imie.message}
                </p>
              )}
            </div>

            <div>
              <Label htmlFor="nazwisko" required>
                Nazwisko
              </Label>
              <Input
                id="nazwisko"
                {...register('nazwisko')}
                error={!!errors.nazwisko}
                placeholder="np. Kowalski"
                className="mt-1.5"
              />
              {errors.nazwisko && (
                <p className="text-sm text-red-500 mt-1">
                  {errors.nazwisko.message}
                </p>
              )}
            </div>

            <div>
              <Label htmlFor="nrPracownika" required>
                Numer pracownika
              </Label>
              <Input
                id="nrPracownika"
                {...register('nrPracownika')}
                error={!!errors.nrPracownika}
                placeholder="np. 12345"
                className="mt-1.5"
              />
              {errors.nrPracownika && (
                <p className="text-sm text-red-500 mt-1">
                  {errors.nrPracownika.message}
                </p>
              )}
            </div>

            <div>
              <Label htmlFor="dataZatrudnienia" required>
                Data zatrudnienia
              </Label>
              <Input
                id="dataZatrudnienia"
                type="date"
                {...register('dataZatrudnienia')}
                error={!!errors.dataZatrudnienia}
                className="mt-1.5"
              />
              {errors.dataZatrudnienia && (
                <p className="text-sm text-red-500 mt-1">
                  {errors.dataZatrudnienia.message}
                </p>
              )}
            </div>
          </div>

          <DialogFooter>
            <Button
              type="button"
              variant="outline"
              onClick={handleClose}
              disabled={createKierowca.isPending}
            >
              Anuluj
            </Button>
            <Button type="submit" isLoading={createKierowca.isPending}>
              Dodaj kierowcę
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
