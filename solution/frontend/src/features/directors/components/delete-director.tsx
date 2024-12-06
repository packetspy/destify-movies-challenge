import { Trash } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { ConfirmationDialog } from '@/components/ui/dialog';
import { useNotifications } from '@/components/ui/notifications';

import { useDeleteDirector } from '../api/delete-director';

type DeleteDirectorProps = {
  uniqueId: string;
};

export const DeleteDirector = ({ uniqueId }: DeleteDirectorProps) => {
  const { addNotification } = useNotifications();
  const mutation = useDeleteDirector({
    mutationConfig: {
      onSuccess: () => {
        addNotification({
          type: 'success',
          title: 'Director Deleted',
        });
      },
    },
  });

  return (
      <ConfirmationDialog
        icon="danger"
        title="Delete Director"
        body="Are you sure you want to delete this Director?"
        triggerButton={
          <Button variant="destructive">
            <Trash className="size-4" />
          </Button>
        }
        confirmButton={
          <Button
            type="button"
            variant="destructive"
            onClick={() =>
                mutation.mutate({ uniqueId })
            }
          >
            Delete Director
          </Button>
        }
      />
  );
};
