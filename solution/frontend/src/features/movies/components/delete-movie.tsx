import { Trash } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { ConfirmationDialog } from '@/components/ui/dialog';
import { useNotifications } from '@/components/ui/notifications';

import { useDeleteMovie } from '../api/delete-movie';

type DeleteMovieProps = {
  movieUniqueId: string;
};

export const DeleteMovie = ({ movieUniqueId }: DeleteMovieProps) => {
  const { addNotification } = useNotifications();
  const deleteMovieMutation = useDeleteMovie({
    mutationConfig: {
      onSuccess: () => {
        addNotification({
          type: 'success',
          title: 'Movie Deleted',
        });
      },
    },
  });

  return (
      <ConfirmationDialog
        icon="danger"
        title="Delete Movie"
        body="Are you sure you want to delete this movie?"
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
              deleteMovieMutation.mutate({ movieUniqueId: movieUniqueId })
            }
          >
            Delete Movie
          </Button>
        }
      />
  );
};
