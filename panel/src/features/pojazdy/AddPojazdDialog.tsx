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
import { useCreatePojazd } from '@/hooks/usePojazdy';
import type { UtworzPojazdDto } from '@/types/pojazd.types';

const pojazdSchema = z.object({
  idModelu: z.string().min(1, 'ID modelu jest wymagane'),
  numerBoczny: z.string().min(1, 'Numer boczny jest wymagany'),
  vin: z.string().min(17, 'VIN musi mieć co najmniej 17 znaków').max(17, 'VIN może mieć maksymalnie 17 znaków'),
  rokProdukcji: z.string().min(1, 'Rok produkcji jest wymagany'),
  dataZakupu: z.string().min(1, 'Data zakupu jest wymagana'),
});

type PojazdFormData = z.infer<typeof pojazdSchema>;

interface AddPojazdDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

export function AddPojazdDialog({ open, onOpenChange }: AddPojazdDialogProps) {
  const createPojazd = useCreatePojazd();

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<PojazdFormData>({
    resolver: zodResolver(pojazdSchema),
  });

  const onSubmit = async (data: PojazdFormData) => {
    try {
      const payload: UtworzPojazdDto = {
        idModelu: parseInt(data.idModelu, 10),
        numerBoczny: data.numerBoczny,
        vin: data.vin,
        rokProdukcji: parseInt(data.rokProdukcji, 10),
        dataZakupu: data.dataZakupu,
      };

      await createPojazd.mutateAsync(payload);
      reset();
      onOpenChange(false);
    } catch (error) {
      console.error('Błąd podczas tworzenia pojazdu:', error);
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
          <DialogTitle>Dodaj nowy pojazd</DialogTitle>
          <DialogDescription>
            Wypełnij formularz, aby dodać nowy pojazd do floty
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="space-y-4">
            <div>
              <Label htmlFor="idModelu" required>
                ID Modelu
              </Label>
              <Input
                id="idModelu"
                type="number"
                {...register('idModelu')}
                error={!!errors.idModelu}
                placeholder="np. 1"
                className="mt-1.5"
              />
              {errors.idModelu && (
                <p className="text-sm text-red-500 mt-1">
                  {errors.idModelu.message}
                </p>
              )}
            </div>

            <div>
              <Label htmlFor="numerBoczny" required>
                Numer boczny
              </Label>
              <Input
                id="numerBoczny"
                {...register('numerBoczny')}
                error={!!errors.numerBoczny}
                placeholder="np. 1234"
                className="mt-1.5"
              />
              {errors.numerBoczny && (
                <p className="text-sm text-red-500 mt-1">
                  {errors.numerBoczny.message}
                </p>
              )}
            </div>

            <div>
              <Label htmlFor="vin" required>
                VIN
              </Label>
              <Input
                id="vin"
                {...register('vin')}
                error={!!errors.vin}
                placeholder="np. 1HGBH41JXMN109186"
                maxLength={17}
                className="mt-1.5"
              />
              {errors.vin && (
                <p className="text-sm text-red-500 mt-1">
                  {errors.vin.message}
                </p>
              )}
            </div>

            <div>
              <Label htmlFor="rokProdukcji" required>
                Rok produkcji
              </Label>
              <Input
                id="rokProdukcji"
                type="number"
                {...register('rokProdukcji')}
                error={!!errors.rokProdukcji}
                placeholder="np. 2020"
                className="mt-1.5"
              />
              {errors.rokProdukcji && (
                <p className="text-sm text-red-500 mt-1">
                  {errors.rokProdukcji.message}
                </p>
              )}
            </div>

            <div>
              <Label htmlFor="dataZakupu" required>
                Data zakupu
              </Label>
              <Input
                id="dataZakupu"
                type="date"
                {...register('dataZakupu')}
                error={!!errors.dataZakupu}
                className="mt-1.5"
              />
              {errors.dataZakupu && (
                <p className="text-sm text-red-500 mt-1">
                  {errors.dataZakupu.message}
                </p>
              )}
            </div>
          </div>

          <DialogFooter>
            <Button
              type="button"
              variant="outline"
              onClick={handleClose}
              disabled={createPojazd.isPending}
            >
              Anuluj
            </Button>
            <Button type="submit" isLoading={createPojazd.isPending}>
              Dodaj pojazd
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
