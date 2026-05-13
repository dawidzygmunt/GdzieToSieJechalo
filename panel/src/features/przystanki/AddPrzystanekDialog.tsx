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
import { useCreatePrzystanek } from '@/hooks/usePrzystanki';
import type { UtworzPrzystanekDto } from '@/types/przystanek.types';

const przystanekSchema = z.object({
  idDzielnicy: z.string().min(1, 'ID dzielnicy jest wymagane'),
  nazwa: z.string().min(1, 'Nazwa jest wymagana'),
  ulica: z.string().optional(),
  typ: z.string().min(1, 'Typ jest wymagany'),
  wiata: z.boolean(),
});

type PrzystanekFormData = z.infer<typeof przystanekSchema>;

interface AddPrzystanekDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

export function AddPrzystanekDialog({ open, onOpenChange }: AddPrzystanekDialogProps) {
  const createPrzystanek = useCreatePrzystanek();

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<PrzystanekFormData>({
    resolver: zodResolver(przystanekSchema),
    defaultValues: {
      wiata: false,
    },
  });

  const onSubmit = async (data: PrzystanekFormData) => {
    try {
      const payload: UtworzPrzystanekDto = {
        idDzielnicy: parseInt(data.idDzielnicy, 10),
        nazwa: data.nazwa,
        ...(data.ulica && { ulica: data.ulica }),
        typ: data.typ,
        wiata: data.wiata,
      };

      await createPrzystanek.mutateAsync(payload);
      reset();
      onOpenChange(false);
    } catch (error) {
      console.error('Błąd podczas tworzenia przystanku:', error);
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
          <DialogTitle>Dodaj nowy przystanek</DialogTitle>
          <DialogDescription>
            Wypełnij formularz, aby dodać nowy przystanek do sieci komunikacyjnej
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="space-y-4">
            <div>
              <Label htmlFor="idDzielnicy" required>
                ID Dzielnicy
              </Label>
              <Input
                id="idDzielnicy"
                type="number"
                {...register('idDzielnicy')}
                error={!!errors.idDzielnicy}
                placeholder="np. 1"
                className="mt-1.5"
              />
              {errors.idDzielnicy && (
                <p className="text-sm text-red-500 mt-1">
                  {errors.idDzielnicy.message}
                </p>
              )}
            </div>

            <div>
              <Label htmlFor="nazwa" required>
                Nazwa
              </Label>
              <Input
                id="nazwa"
                {...register('nazwa')}
                error={!!errors.nazwa}
                placeholder="np. Plac Centralny"
                className="mt-1.5"
              />
              {errors.nazwa && (
                <p className="text-sm text-red-500 mt-1">
                  {errors.nazwa.message}
                </p>
              )}
            </div>

            <div>
              <Label htmlFor="ulica">Ulica</Label>
              <Input
                id="ulica"
                {...register('ulica')}
                placeholder="np. ul. Główna"
                className="mt-1.5"
              />
            </div>

            <div>
              <Label htmlFor="typ" required>
                Typ
              </Label>
              <Input
                id="typ"
                {...register('typ')}
                error={!!errors.typ}
                placeholder="np. Autobus, Tramwaj"
                className="mt-1.5"
              />
              {errors.typ && (
                <p className="text-sm text-red-500 mt-1">
                  {errors.typ.message}
                </p>
              )}
            </div>

            <div className="flex items-center gap-2">
              <input
                id="wiata"
                type="checkbox"
                {...register('wiata')}
                className="h-4 w-4 rounded border-slate-300 text-primary-500 focus:ring-2 focus:ring-primary-500"
              />
              <Label htmlFor="wiata" className="mb-0">
                Wiata
              </Label>
            </div>
          </div>

          <DialogFooter>
            <Button
              type="button"
              variant="outline"
              onClick={handleClose}
              disabled={createPrzystanek.isPending}
            >
              Anuluj
            </Button>
            <Button type="submit" isLoading={createPrzystanek.isPending}>
              Dodaj przystanek
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
