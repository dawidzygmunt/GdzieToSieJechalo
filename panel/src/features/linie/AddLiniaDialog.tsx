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
import { useCreateLinia } from '@/hooks/useLinie';
import type { UtworzLinieDto } from '@/types/linia.types';

const liniaSchema = z.object({
  numerLinii: z.string().min(1, 'Numer linii jest wymagany'),
  typLinii: z.string().min(1, 'Typ linii jest wymagany'),
  opis: z.string().optional(),
});

type LiniaFormData = z.infer<typeof liniaSchema>;

interface AddLiniaDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

export function AddLiniaDialog({ open, onOpenChange }: AddLiniaDialogProps) {
  const createLinia = useCreateLinia();

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<LiniaFormData>({
    resolver: zodResolver(liniaSchema),
  });

  const onSubmit = async (data: LiniaFormData) => {
    try {
      const payload: UtworzLinieDto = {
        numerLinii: data.numerLinii,
        typLinii: data.typLinii,
        ...(data.opis && { opis: data.opis }),
      };

      await createLinia.mutateAsync(payload);
      reset();
      onOpenChange(false);
    } catch (error) {
      console.error('Błąd podczas tworzenia linii:', error);
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
          <DialogTitle>Dodaj nową linię</DialogTitle>
          <DialogDescription>
            Wypełnij formularz, aby dodać nową linię komunikacyjną
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="space-y-4">
            <div>
              <Label htmlFor="numerLinii" required>
                Numer linii
              </Label>
              <Input
                id="numerLinii"
                {...register('numerLinii')}
                error={!!errors.numerLinii}
                placeholder="np. 123"
                className="mt-1.5"
              />
              {errors.numerLinii && (
                <p className="text-sm text-red-500 mt-1">
                  {errors.numerLinii.message}
                </p>
              )}
            </div>

            <div>
              <Label htmlFor="typLinii" required>
                Typ linii
              </Label>
              <Input
                id="typLinii"
                {...register('typLinii')}
                error={!!errors.typLinii}
                placeholder="np. Autobus, Tramwaj"
                className="mt-1.5"
              />
              {errors.typLinii && (
                <p className="text-sm text-red-500 mt-1">
                  {errors.typLinii.message}
                </p>
              )}
            </div>

            <div>
              <Label htmlFor="opis">Opis</Label>
              <Input
                id="opis"
                {...register('opis')}
                placeholder="Opcjonalny opis linii"
                className="mt-1.5"
              />
            </div>
          </div>

          <DialogFooter>
            <Button
              type="button"
              variant="outline"
              onClick={handleClose}
              disabled={createLinia.isPending}
            >
              Anuluj
            </Button>
            <Button type="submit" isLoading={createLinia.isPending}>
              Dodaj linię
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
