import { Pen } from 'lucide-react';

import { Button } from '@/components/ui/button';
import { Form, FormDrawer, Input, Textarea } from '@/components/ui/form';
import { useNotifications } from '@/components/ui/notifications';

import { useMovie } from '../api/get-movie';
import { updateMovieInputSchema,   useUpdateMovie } from '../api/update-movie';

type UpdateMovieProps = {
  movieUniqueId: string;
};

export const UpdateMovie = ({ movieUniqueId }: UpdateMovieProps) => {

  console.log('UpdateMovie aquiiiiiiiiiiiiiii', movieUniqueId);

  const { addNotification } = useNotifications();
  const movieQuery = useMovie({ movieUniqueId });
  const updateMovieMutation = useUpdateMovie({
    mutationConfig: {
      onSuccess: () => {
        addNotification({
          type: 'success',
          title: 'Movie Updated',
        });
      },
    },
  });

  const movie = movieQuery.data?.data;

  return (
      <FormDrawer
        isDone={updateMovieMutation.isSuccess}
        triggerButton={
          <Button size="sm">
            <Pen className="size-4" />
            Update Movie
          </Button>
        }
        title="Update Movie"
        submitButton={
          <Button
            form="update-movie"
            type="submit"
            size="sm"
          >
            Submit
          </Button>
        }
      >
        <Form
          id="update-movie"
          onSubmit={(values) => {
            console.log('movieUniqueId aquiiiiiiiiiiiiiii', movieUniqueId);
            updateMovieMutation.mutate({
              data: values,
              movieUniqueId,
            });
          }}
          options={{
            defaultValues: {
              title: movie?.title ?? '',
              year : movie?.year ?? 0,
              rated: movie?.rated ?? '',
              genre: movie?.genre ?? '',
              language: movie?.language ?? '',
              country: movie?.country ?? '',
              poster: movie?.poster ?? '',
              plot: movie?.plot ?? '',
            },
          }}
          schema={updateMovieInputSchema}
        >
          {({ register, formState }) => (
            <>
            <Input
              label="Title"
              error={formState.errors['title']}
              registration={register('title')}
            />

            <Input
              label="Year"
              error={formState.errors['year']}
              registration={register('year')}
              type='number'
            />

            <Input
              label="Rated"
              error={formState.errors['rated']}
              registration={register('rated')}
            />

            <Input
              label="Genre"
              error={formState.errors['genre']}
              registration={register('genre')}
            />

            <Input
              label="Language"
              error={formState.errors['language']}
              registration={register('language')}
            />

            <Input
              label="Country"
              error={formState.errors['country']}
              registration={register('country')}
            />

            <Input
              label="Poster"
              error={formState.errors['poster']}
              registration={register('poster')}
            />

            <Textarea
              label="Plot"
              error={formState.errors['plot']}
              registration={register('plot')}
            />
          </>
          )}
        </Form>
      </FormDrawer>
  );
};
