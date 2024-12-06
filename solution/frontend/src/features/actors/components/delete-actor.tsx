import { Trash } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { ConfirmationDialog } from '@/components/ui/dialog';
import { useNotifications } from '@/components/ui/notifications';

import { useDeleteActor } from '../api/delete-actor';

type DeleteActorProps = {
  ActorUniqueId: string;
};

export const DeleteActor = ({ ActorUniqueId }: DeleteActorProps) => {
  const { addNotification } = useNotifications();
  const deleteActorMutation = useDeleteActor({
    mutationConfig: {
      onSuccess: () => {
        addNotification({
          type: 'success',
          title: 'Actor Deleted',
          delay: 1000
        });
      },
    },
  });

  return (
      <ConfirmationDialog
        icon="danger"
        title="Delete Actor"
        body="Are you sure you want to delete this Actor?"
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
              deleteActorMutation.mutate({ ActorUniqueId: ActorUniqueId })
            }
          >
            Delete Actor
          </Button>
        }
      />
  );
};
